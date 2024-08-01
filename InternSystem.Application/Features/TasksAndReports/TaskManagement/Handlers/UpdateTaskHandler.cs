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
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, TaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateTaskHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<TaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Tasks? existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(request.Id);
                if (existingTask == null || existingTask.IsDelete == true || existingTask.HoanThanh == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");

                if (request.DuAnId > 0)
                {
                    DuAn? duAn = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId);
                    if (duAn == null || duAn.IsDelete == true)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                    if (request.HanHoanThanh > duAn.ThoiGianKetThuc || request.NgayGiao < duAn.ThoiGianBatDau)
                        throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Thời gian của task không nằm trong khoảng thời gian của dự án");

                    existingTask.DuAnId = (int)request.DuAnId;
                }

                //IEnumerable<Tasks>? exist2 = await _unitOfWork.TaskRepository.GetAllAsync();
                //List<Tasks>? list = exist2.ToList();
                //foreach (var item in list)
                //{
                //    if (item.MoTa == request.MoTa && item.DuAnId == request.DuAnId)
                //        throw new ArgumentNullException(
                //         nameof(request), $"{request.MoTa} đã tồn tại với dự án {request.DuAnId}");
                //}

                // Kiểm tra thời gian hoàn thành và ngày giao task 
                if (request.HanHoanThanh < request.NgayGiao || existingTask.NgayGiao > request.HanHoanThanh
                    || request.NgayGiao > existingTask.HanHoanThanh)
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Thời gian hoàn thành phải sau ngày giao");

                if (!string.IsNullOrWhiteSpace(request.MoTa))
                    existingTask.MoTa = request.MoTa;
                if (!string.IsNullOrWhiteSpace(request.NoiDung))
                    existingTask.NoiDung = request.NoiDung;
                if (request.NgayGiao.HasValue)
                    existingTask.NgayGiao = request.NgayGiao.Value;
                if (request.HanHoanThanh.HasValue)
                    existingTask.HanHoanThanh = request.HanHoanThanh.Value;
                if (request.HoanThanh.HasValue)
                    existingTask.HoanThanh = request.HoanThanh.Value;

                existingTask.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingTask.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<TaskResponse>(existingTask);
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
