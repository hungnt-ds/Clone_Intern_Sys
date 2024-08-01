using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Handlers
{
    public class CreateUserDuAnHandler : IRequestHandler<CreateUserDuAnCommand, CreateUserDuAnResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateUserDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateUserDuAnResponse> Handle(CreateUserDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDuAn = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId);
                if (existingDuAn == null || existingDuAn.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null || user.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");

                var vitri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.IdViTri);
                if (vitri == null || vitri.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");


                UserDuAn newUserDuAn = _mapper.Map<UserDuAn>(request);
                newUserDuAn.CreatedBy = _userContextService.GetCurrentUserId();
                newUserDuAn.LastUpdatedBy = newUserDuAn.CreatedBy;
                newUserDuAn.CreatedTime = _timeService.SystemTimeNow;
                newUserDuAn.LastUpdatedTime = _timeService.SystemTimeNow;
                newUserDuAn = await _unitOfWork.UserDuAnRepository.AddAsync(newUserDuAn);

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateUserDuAnResponse>(newUserDuAn);
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
