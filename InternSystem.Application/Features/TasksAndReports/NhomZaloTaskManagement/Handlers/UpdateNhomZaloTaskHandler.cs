using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Handlers
{
    public class UpdateNhomZaloTaskHandler : IRequestHandler<UpdateNhomZaloTaskCommand, NhomZaloTaskReponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public UpdateNhomZaloTaskHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<NhomZaloTaskReponse> Handle(UpdateNhomZaloTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingNhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.GetByIdAsync(request.Id);
                if (existingNhomZaloTask == null || existingNhomZaloTask.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Nhóm Zalo này không thực hiện task này");
                }

                if (existingNhomZaloTask.TaskId == request.TaskId && existingNhomZaloTask.NhomZaloId == request.NhomZaloId)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Nhóm đã nhận task này");
                }

                await ValidateAndAssignTaskIdAsync(request, existingNhomZaloTask);
                await ValidateAndAssignNhomZaloIdAsync(request, existingNhomZaloTask);

                UpdateTrangThai(existingNhomZaloTask, request.TrangThai);
                if (IsDoneStatus(request.TrangThai))
                {
                    await MarkTaskAsCompletedAsync(request.TaskId);
                }

                existingNhomZaloTask.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingNhomZaloTask.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<NhomZaloTaskReponse>(existingNhomZaloTask);
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

        private async Task ValidateAndAssignTaskIdAsync(UpdateNhomZaloTaskCommand request, NhomZaloTask exist)
        {
            if (request.TaskId.HasValue)
            {
                Tasks task = await _unitOfWork.TaskRepository.GetByIdAsync(request.TaskId.Value);
                if (task == null || task.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");
                }

                exist.TaskId = request.TaskId.Value;
            }
        }

        private async Task ValidateAndAssignNhomZaloIdAsync(UpdateNhomZaloTaskCommand request, NhomZaloTask exist)
        {
            if (request.NhomZaloId.HasValue)
            {
                NhomZalo nhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.NhomZaloId.Value);
                if (nhomZalo == null || nhomZalo.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không tìm thấy nhóm Zalo");
                }

                exist.NhomZaloId = request.NhomZaloId.Value;
            }
        }

        private void UpdateTrangThai(NhomZaloTask nhomZaloTask, string trangThai)
        {
            if (!string.IsNullOrWhiteSpace(trangThai) && IsValidTrangThai(trangThai))
            {
                nhomZaloTask.TrangThai = trangThai;
            }
        }

        private bool IsValidTrangThai(string trangThai)
        {
            var validTrangThai = new[]
            {
                _config["Trangthai:Active"],
                _config["Trangthai:Completed"],
                _config["Trangthai:Cancelled"],
                _config["Trangthai:Pending"]
            };

            return validTrangThai.Any(status => trangThai.Contains(status, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsDoneStatus(string trangThai)
        {
            return trangThai.Contains(_config["Trangthai:Completed"]!, StringComparison.OrdinalIgnoreCase);
        }

        private async Task MarkTaskAsCompletedAsync(int? taskId)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task != null)
            {
                task.HoanThanh = true;
            }
        }
    }
}
