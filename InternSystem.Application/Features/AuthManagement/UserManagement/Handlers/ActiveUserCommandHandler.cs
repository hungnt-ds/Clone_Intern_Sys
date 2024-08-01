using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class ActiveUserCommandHandler : IRequestHandler<ActiveUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ITimeService _timeService;
        private readonly IUserContextService _userContextService;
        public ActiveUserCommandHandler(IUserContextService userContextService, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AspNetUser> userManager, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _timeService = timeService;
            _userContextService = userContextService;
        }
        public async Task<bool> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                var user = await _unitOfWork.UserRepository.GetByIdAllAsync(request.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} không tìm thấy.");
                }
                var tempUservitri = await _unitOfWork.UserViTriRepository.GetAllAsync();
                var checkDeleteListVitri = tempUservitri.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteListVitri.Any(m => m.UserId == request.UserId))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn intern trong kỳ thực tập này.");
                }
                var tempUserDuan = await _unitOfWork.UserDuAnRepository.GetAllAsync();
                var checkDeleteListDuan = tempUserDuan.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteListDuan.Any(m => m.UserId == request.UserId))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn intern trong kỳ thực tập này.");
                }
                user.IsActive = request.IsActive;
                user.LastUpdatedBy = currentUserId;
                user.LastUpdatedTime = _timeService.SystemTimeNow;

                if (!request.IsActive)
                {
                    user.IsDelete = true;
                    user.DeletedTime = _timeService.SystemTimeNow;
                }
                else
                {
                    user.IsDelete = false;
                    user.DeletedTime = null;
                }
                var userUpdate = _mapper.Map<AspNetUser>(user);
                var result = await _userManager.UpdateAsync(userUpdate);
                return result.Succeeded;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
