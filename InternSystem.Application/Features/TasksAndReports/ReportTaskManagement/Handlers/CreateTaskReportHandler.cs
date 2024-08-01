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
    public class CreateTaskReportHandler : IRequestHandler<CreateTaskReportCommand, TaskReportResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateTaskReportHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<TaskReportResponse> Handle(CreateTaskReportCommand request, CancellationToken cancellationToken)

        {
            try
            {
                AspNetUser? existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (existingUser == null || existingUser.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");

                Tasks? existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
                if (existingTask == null || existingTask.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");

                ReportTask newTask = _mapper.Map<ReportTask>(request);

                newTask.TrangThai = _config["Trangthai:Pending"]!;
                newTask.CreatedTime = _timeService.SystemTimeNow;
                newTask.CreatedBy = _userContextService.GetCurrentUserId();
                newTask.LastUpdatedTime = _timeService.SystemTimeNow;
                newTask.LastUpdatedBy = _userContextService.GetCurrentUserId();
                await _unitOfWork.ReportTaskRepository.AddAsync(newTask);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<TaskReportResponse>(newTask);
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
