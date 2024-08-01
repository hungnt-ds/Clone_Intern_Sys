using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Handlers
{
    public class DeleteUserDuAnHandler : IRequestHandler<DeleteUserDuAnCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteUserDuAnHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteUserDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UserDuAn? existingUserDuAn = await _unitOfWork.UserDuAnRepository.GetByIdAsync(request.Id);
                if (existingUserDuAn == null || existingUserDuAn.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng trong dự án");

                existingUserDuAn.DeletedBy = _userContextService.GetCurrentUserId();
                existingUserDuAn.DeletedTime = _timeService.SystemTimeNow;
                existingUserDuAn.IsActive = false;
                existingUserDuAn.IsDelete = true;

                await _unitOfWork.SaveChangeAsync();
                return true;
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
