using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Handlers
{
    public class DeleteUserTaskHandler : IRequestHandler<DeleteUserTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteUserTaskHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                var systemTimeNow = _timeService.SystemTimeNow;

                UserTask? existingUserTask = await _unitOfWork.UserTaskRepository.GetByIdAsync(request.Id);
                if (existingUserTask == null || existingUserTask.IsDelete == true)
                    return false;

                //AspNetUser user = await _unitOfWork.UserRepository.GetByIdAsync(existingUserTask.UserId);
                //if (user != null)
                //    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không thể xóa ReportTask vì có User liên quan vẫn còn tồn tại.");

                //Tasks task = await _unitOfWork.TaskRepository.GetByIdAsync(existingUserTask.TaskId);
                //if (task != null)
                //    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không thể xóa ReportTask vì có Tasks liên quan vẫn còn tồn tại.");

                existingUserTask.DeletedBy = currentUserId;
                existingUserTask.DeletedTime = systemTimeNow;
                existingUserTask.IsActive = false;
                existingUserTask.IsDelete = true;
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
