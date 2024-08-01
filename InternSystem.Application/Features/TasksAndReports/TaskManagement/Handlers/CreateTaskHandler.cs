using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateTaskHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var curentUserId = _userContextService.GetCurrentUserId();

                DuAn? existingDuAn = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId);
                if (existingDuAn == null || existingDuAn.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                //IEnumerable<Tasks>? exist2 = await _unitOfWork.TaskRepository.GetAllAsync();
                //List<Tasks>? list = exist2.ToList();
                //foreach (var item in list)
                //{
                //    if (item.MoTa == request.MoTa && item.DuAnId == request.DuAnId)
                //        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");
                //}

                //// Kiểm tra thời gian của task và dự án có phù hợp 
                //if (request.HanHoanThanh <= existingDuAn.ThoiGianKetThuc
                //    || request.NgayGiao >= existingDuAn.ThoiGianBatDau)
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Thời gian của task phải nằm trong khoảng thời gian của dự án");

                if (request.NgayGiao < existingDuAn.ThoiGianBatDau
                    || request.HanHoanThanh > existingDuAn.ThoiGianKetThuc)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Thời gian của task phải nằm trong khoảng thời gian của dự án");

                Tasks newTask = _mapper.Map<Tasks>(request);
                newTask.CreatedTime = _timeService.SystemTimeNow;
                newTask.CreatedBy = curentUserId;
                newTask.LastUpdatedTime = _timeService.SystemTimeNow;
                newTask.LastUpdatedBy = curentUserId;
                newTask.HoanThanh = false;
                newTask = await _unitOfWork.TaskRepository.AddAsync(newTask);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<TaskResponse>(newTask);
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
