using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CreateUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITimeService _timeService;
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _timeService = timeService;
        }
        public async Task<CreateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = await _userManager.FindByIdAsync(request.Id);

                if (newUser == null)
                    throw new ArgumentNullException(nameof(request.Id), "Không tìm thấy người dùng");

                if (!string.IsNullOrEmpty(request.HoVaTen))
                    newUser.HoVaTen = request.HoVaTen;


                if (!string.IsNullOrEmpty(request.Email) && request.Email != newUser.Email)
                {
                    newUser.Email = request.Email;
                }

                if (!string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != newUser.PhoneNumber)
                {
                    newUser.PhoneNumber = request.PhoneNumber;
                }

                if (request.InternInfoId != null)
                {
                    if (await _unitOfWork.InternInfoRepository.GetByIdAsync(request.InternInfoId) == null)
                        throw new ArgumentNullException(nameof(request.InternInfoId), "Không tìm thấy thông tin thực tập sinh");

                    newUser.InternInfoId = request.InternInfoId;
                }
                AspNetUser userUpdate = _mapper.Map<AspNetUser>(newUser);
                userUpdate.LastUpdatedTime = _timeService.SystemTimeNow;
                var resultUpdate = await _userManager.UpdateAsync(userUpdate);

                if (!resultUpdate.Succeeded)
                    throw new InvalidOperationException(resultUpdate.Errors.FirstOrDefault()?.Description);
                //=====================================================================================================
                // Get current roles
                var currentRoles = await _userManager.GetRolesAsync(newUser);

                // Find new role
                if (!string.IsNullOrEmpty(request.RoleName))
                {
                    var roleName = request.RoleName.Trim();
                    var newRole = await _roleManager.FindByNameAsync(roleName);
                    if (newRole == null)
                    {
                        throw new ArgumentException("Không tìm thấy vai trò");
                    }

                    // Remove current roles
                    var removeResult = await _userManager.RemoveFromRolesAsync(newUser, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Lỗi xóa vai trò người dùng: {removeResult.Errors.FirstOrDefault()?.Description}");
                    }

                    // Add new role
                    var addResult = await _userManager.AddToRoleAsync(newUser, newRole.Name);
                    if (!addResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Lỗi khi thêm người dùng vào vai trò: {addResult.Errors.FirstOrDefault()?.Description}");
                    }
                }
                //=====================================================================================================
                return _mapper.Map<CreateUserResponse>(userUpdate);
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
