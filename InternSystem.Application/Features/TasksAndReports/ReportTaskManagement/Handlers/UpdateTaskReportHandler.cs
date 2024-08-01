using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Handlers
{
    public class UpdateTaskReportHandler : IRequestHandler<UpdateTaskReportCommand, TaskReportResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateTaskReportHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config,
            IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<TaskReportResponse> Handle(UpdateTaskReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReportTask? existingReportTask = await _unitOfWork.ReportTaskRepository.GetByIdAsync(request.Id);
                if (existingReportTask == null || existingReportTask.IsDelete == true)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task report");
                }

                if (request.TaskId != null)
                {
                    Tasks? tasks = await _unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
                    if (tasks == null || tasks.IsDelete == true)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");
                }
                if (!string.IsNullOrWhiteSpace(request.UserId))
                {
                    AspNetUser? user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                    if (user == null || user.IsDelete == true)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }
                if (!string.IsNullOrWhiteSpace(request.TrangThai) && (
                    request.TrangThai.ToLower().Contains(_config["Trangthai:Active"]!.ToLower()) ||
                    request.TrangThai.ToLower().Contains(_config["Trangthai:Completed"]!.ToLower()) ||
                    request.TrangThai.ToLower().Contains(_config["Trangthai:Cancelled"]!.ToLower()) ||
                    request.TrangThai.ToLower().Contains(_config["Trangthai:Pending"]!.ToLower())))
                    existingReportTask.TrangThai = request.TrangThai;
                if (!string.IsNullOrWhiteSpace(request.MoTa))
                    existingReportTask.MoTa = request.MoTa;
                if (!string.IsNullOrWhiteSpace(request.NoiDungBaoCao))
                { existingReportTask.NoiDungBaoCao = request.NoiDungBaoCao; }

                existingReportTask.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingReportTask.NgayBaoCao = _timeService.SystemTimeNow.DateTime;
                existingReportTask.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<TaskReportResponse>(existingReportTask);
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
