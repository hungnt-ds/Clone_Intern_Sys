using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Handlers
{
    public class DeleteTaskReportHandler : IRequestHandler<DeleteTaskReportCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteTaskReportHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteTaskReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReportTask? existingReportTask = await _unitOfWork.ReportTaskRepository.GetByIdAsync(request.Id);
                if (existingReportTask == null || existingReportTask.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy report của task");

                // Kiểm tra trước khi xóa id có ở bảng khác không (không cần nữa)
                //AspNetUser user = await _unitOfWork.UserRepository.GetByIdAsync(existingReportTask.UserId);
                //if (user != null)
                //    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không thể xóa report của task vì có người dùng liên quan vẫn còn tồn tại.");

                //Tasks task = await _unitOfWork.TaskRepository.GetByIdAsync(existingReportTask.TaskId);
                //if (task != null)
                //    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không thể xóa report của task vì có task liên quan vẫn còn tồn tại.");

                existingReportTask.DeletedBy = _userContextService.GetCurrentUserId();
                existingReportTask.DeletedTime = _timeService.SystemTimeNow;
                existingReportTask.IsActive = false;
                existingReportTask.IsDelete = true;
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
