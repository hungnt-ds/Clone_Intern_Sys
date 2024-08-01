using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Handlers
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteTaskHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Tasks? existTask = await _unitOfWork.TaskRepository.GetByIdAsync(request.Id);
                if (existTask == null || existTask.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy task");

                // Kiểm tra trước khi xóa id có ở bảng UserTask không
                var tempUserTask = await _unitOfWork.UserTaskRepository.GetAllAsync();
                var checkDeleteUserTask = tempUserTask.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteUserTask.Any(m => m.TaskId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa task vì có user task liên quan vẫn còn tồn tại.");
                }

                // Kiểm tra trước khi xóa id có ở bảng ReportTask không
                var tempReportTask = await _unitOfWork.ReportTaskRepository.GetAllAsync();
                var checkDeleteReportTask = tempReportTask.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteReportTask.Any(m => m.TaskId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa task vì có report task liên quan vẫn còn tồn tại.");
                }

                // Kiểm tra trước khi xóa id có ở bảng GroupZaloTask không
                var tempNhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.GetAllAsync();
                var checkDeleteNhomZaloTask = tempNhomZaloTask.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteNhomZaloTask.Any(m => m.TaskId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa task vì có group zalo task liên quan vẫn còn tồn tại.");
                }

                existTask.DeletedBy = _userContextService.GetCurrentUserId();
                existTask.DeletedTime = _timeService.SystemTimeNow;
                existTask.IsActive = false;
                existTask.IsDelete = true;
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
