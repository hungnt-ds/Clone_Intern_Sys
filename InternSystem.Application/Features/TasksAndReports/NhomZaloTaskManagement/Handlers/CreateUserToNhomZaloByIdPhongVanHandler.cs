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

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Handlers
{
    public class CreateUserToNhomZaloByIdPhongVanHandler : IRequestHandler<CreateUserToNhomZaloByIdCommand, UserNhomZaloTaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateUserToNhomZaloByIdPhongVanHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config,
            IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UserNhomZaloTaskResponse> Handle(CreateUserToNhomZaloByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LichPhongVan? existingLichPhongVan = await _unitOfWork.LichPhongVanRepository.GetByIdAsync(request.Id);
                if (existingLichPhongVan == null || existingLichPhongVan.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");

                var ketqua = _config.GetSection("RoleSettings");
                if (existingLichPhongVan.KetQua != ketqua["Intern"] && existingLichPhongVan.KetQua != ketqua["Leader"] && existingLichPhongVan.KetQua == null)
                {
                    if (existingLichPhongVan.KetQua == _config["Ketqua:Chuadat"])
                        throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Kết quả phỏng vấn {request.Id} chưa đạt");
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Kết quả phỏng vấn {request.Id} không tồn tại");
                }
                var groupName = await _unitOfWork.NhomZaloRepository.GetNhomZalosByNameAsync(existingLichPhongVan.KetQua);
                if (groupName == null || groupName.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Kết quả của intern {existingLichPhongVan.KetQua}");

                InternInfo? internInfo = await _unitOfWork.InternInfoRepository.GetByIdAsync(existingLichPhongVan.IdNguoiDuocPhongVan);
                if (internInfo == null || internInfo.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Intern không tồn tại trong lịch phỏng vấn");

                var userNhomZalo = new UserNhomZalo
                {
                    UserId = internInfo.UserId,
                    IdNhomZaloChung = groupName.Id,
                    IsMentor = false,
                    IsLeader = false,
                    CreatedBy = _userContextService.GetCurrentUserId(),
                    LastUpdatedBy = _userContextService.GetCurrentUserId(),
                    CreatedTime = _timeService.SystemTimeNow,
                    LastUpdatedTime = _timeService.SystemTimeNow
                };
                await _unitOfWork.UserNhomZaloRepository.AddAsync(userNhomZalo);
                var nhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.GetTaskByNhomZaloIdAsync(groupName.Id);
                foreach (var item in nhomZaloTask)
                {
                    var userTaskRequest = new CreateUserTaskCommand
                    {
                        UserId = internInfo.UserId,
                        TaskId = item.TaskId,
                    };

                    var newUserTask = _mapper.Map<UserTask>(userTaskRequest);
                    newUserTask.TrangThai = "Pending";
                    newUserTask.LastUpdatedBy = _userContextService.GetCurrentUserId();
                    newUserTask.CreatedTime = _timeService.SystemTimeNow;
                    newUserTask.LastUpdatedTime = _timeService.SystemTimeNow;
                    await _unitOfWork.UserTaskRepository.AddAsync(newUserTask);
                }
                var taskDtos = nhomZaloTask.Select(t => new TaskDto
                {
                    TaskId = t.Id,
                    TaskName = t.Tasks.MoTa
                }).ToList();

                var User = new UserNhomZaloTaskResponse
                {
                    InterviewId = existingLichPhongVan.Id,
                    KetQua = existingLichPhongVan.KetQua,
                    GroupName = groupName.TenNhom,
                    GroupLink = groupName.LinkNhom,
                    CreateBy = _userContextService.GetCurrentUserId(),
                    Tasks = taskDtos
                };
                await _unitOfWork.SaveChangeAsync();
                return User;
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
