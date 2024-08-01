using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using InternSystem.Application.Features.InternManagement.InternManagement.Utility;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Handlers
{
    public class ImportCsvHandler : IRequestHandler<ImportCsvCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly ITimeService _timeService;

        public ImportCsvHandler(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AspNetUser> userManager,
            IEmailService emailService, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IFileService fileService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _configuration = configuration;
            _fileService = fileService;
            _timeService = timeService;
        }
        //Save InternInfo first then save AspNetUser
        public async Task<Unit> Handle(ImportCsvCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Read csv file using injected fileService
                var records = _fileService.ReadFromCsv(request.File.OpenReadStream());

                Console.WriteLine($"Number of records to process: {records.Count}");

                foreach (var record in records)
                {
                    // Save InternInfo first
                    var internInfo = _mapper.Map<InternInfo>(record);
                    internInfo.EmailCaNhan = record.Email;
                    internInfo.Sdt = record.PhoneNumber;
                    internInfo.CreatedTime = _timeService.SystemTimeNow;
                    internInfo.LastUpdatedTime = _timeService.SystemTimeNow;
                    internInfo.CreatedBy = "System"; // placeholder, replace with actual user ID from token
                    internInfo.LastUpdatedBy = "System"; // placeholder, replace with actual user ID from token

                    var internResult = await _unitOfWork.InternInfoRepository.AddAsync(internInfo);
                    await _unitOfWork.SaveChangeAsync();

                    // Save AspNetUser
                    var user = _mapper.Map<AspNetUser>(record);
                    user.UserName = user.Email;
                    user.InternInfoId = internInfo.Id;
                    user.HoVaTen = internInfo.HoTen;
                    user.CreatedTime = _timeService.SystemTimeNow;
                    user.LastUpdatedTime = _timeService.SystemTimeNow;
                    //Generate random password for user
                    var randomPassword = PasswordGenerator.Generate(8);

                    var result = await _userManager.CreateAsync(user, randomPassword);

                    // Check if user add successfully
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(e => $"{e.Code}: {e.Description}");
                        var errorMessage = string.Join("; ", errors);
                        // Rollback InternInfo creation
                        _unitOfWork.InternInfoRepository.Remove(internInfo);
                        await _unitOfWork.SaveChangeAsync();
                        throw new Exception($"Failed to create user: {errorMessage}");
                    }
                    else
                    {
                        // Retrieve role name from appsettings
                        var internRole = _configuration["RoleSettings:Intern"];
                        // role exists in the database
                        var roleExists = await _roleManager.RoleExistsAsync(internRole);
                        if (!roleExists)
                        {
                            throw new Exception($"Role '{internRole}' does not exist in the database");
                        }
                        await _userManager.AddToRoleAsync(user, internRole);

                        internInfo.UserId = user.Id; // Update internInfo with the user ID
                        await _unitOfWork.SaveChangeAsync(); // Save the changes to internInfo

                        // Send email to user
                        var emailBody = $"Your account has been created successfully. \nUsername: {user.UserName}\nPassword: {randomPassword}";
                        var emailSent = await _emailService.SendEmailAsync(new List<string> { user.Email }, "Account Creation Notification", emailBody);

                        if (!emailSent)
                        {
                            throw new Exception($"Failed to send email notification to {user.Email}");
                        }
                    }
                }
                return Unit.Value;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lưu.");
            }
        }
    }
}