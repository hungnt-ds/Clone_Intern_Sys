using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Handlers
{
    public class UpdateUserTaskHandler : IRequestHandler<UpdateUserTaskCommand, UserTaskReponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateUserTaskHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config,
            IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UserTaskReponse> Handle(UpdateUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UserTask existingUserTask = await _unitOfWork.UserTaskRepository.GetByIdAsync(request.Id);
                if (existingUserTask == null || existingUserTask.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng trong task cần chỉnh sửa");
                }

                await ValidateAndAssignUserTaskAsync(request, existingUserTask);

                if (!string.IsNullOrWhiteSpace(request.TrangThai))
                {
                    UpdateTrangThai(request, existingUserTask);
                }

                existingUserTask.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingUserTask.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UserTaskReponse>(existingUserTask);
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

        private async Task ValidateAndAssignUserTaskAsync(UpdateUserTaskCommand request, UserTask exist)
        {
            if (request.TaskId != null)
            {
                // Validate and assign TaskId
                var tasks = await _unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
                if (tasks == null || tasks.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");
                }

                // Check if the combination of TaskId and UserId already exists
                var existingUserTasks = await _unitOfWork.UserTaskRepository.GetAllAsync();
                if (existingUserTasks.Any(ut => ut.TaskId == request.TaskId && ut.UserId == request.UserId && ut.Id != exist.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Người dùng đã được đăng ký cho task");
                }

                exist.TaskId = (int)request.TaskId;
            }

            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                // Validate and assign UserId
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null || user.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người dùng không tồn tại");
                }

                exist.UserId = request.UserId;
            }
        }

        private void UpdateTrangThai(UpdateUserTaskCommand request, UserTask exist)
        {
            var trangThaiLower = request.TrangThai.ToLower();
            var validTrangThai = new[] {
                _config["Trangthai:Active"]?.ToLower(),
                _config["Trangthai:Completed"]?.ToLower(),
                _config["Trangthai:Cancelled"]?.ToLower(),
                _config["Trangthai:Pending"]?.ToLower()
            };

            if (Array.Exists(validTrangThai, status => trangThaiLower.Contains(status)))
            {
                exist.TrangThai = request.TrangThai;

                if (trangThaiLower.Contains(_config["Trangthai:Completed"]!.ToLower()))
                {
                    MarkTaskAsCompleteAsync(request.TaskId);
                }
            }
        }

        private async Task MarkTaskAsCompleteAsync(int? taskId)
        {
            if (taskId.HasValue)
            {
                var tasks = await _unitOfWork.TaskRepository.GetByIdAsync(taskId.Value);
                if (tasks != null)
                {
                    tasks.HoanThanh = true;
                }
            }
        }
    }
}