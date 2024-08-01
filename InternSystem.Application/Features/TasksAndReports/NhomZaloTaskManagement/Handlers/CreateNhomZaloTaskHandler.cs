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
    public class CreateNhomZaloTaskHandler : IRequestHandler<CreateNhomZaloTaskCommand, NhomZaloTaskReponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public CreateNhomZaloTaskHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<NhomZaloTaskReponse> Handle(CreateNhomZaloTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                var systemTimeNow = _timeService.SystemTimeNow;

                NhomZalo? existingNhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.NhomZaloId);
                if (existingNhomZalo == null || existingNhomZalo.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm zalo");

                Tasks? existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
                if (existingTask == null || existingTask.IsDelete || existingTask.HoanThanh)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");

                IEnumerable<NhomZaloTask>? allNhomZalo = await _unitOfWork.NhomZaloTaskRepository.GetAllAsync();
                List<NhomZaloTask>? listNhomZalo = allNhomZalo.ToList();
                foreach (var nhomZalo in listNhomZalo)
                {
                    if (nhomZalo.TaskId == request.TaskId && nhomZalo.NhomZaloId == request.NhomZaloId)
                        throw new ErrorException(
                            StatusCodes.Status400BadRequest, 
                            ResponseCodeConstants.BADREQUEST, 
                            $"{request.TaskId} đã tồn tại với nhóm zalo {request.NhomZaloId}"
                        );
                }
                IEnumerable<UserNhomZalo> userNhomZalos = await _unitOfWork.UserNhomZaloRepository.GetByNhomZaloIdAsync(request.NhomZaloId);

                // gan nhom zalo vao task
                NhomZaloTask newNhomZaloTask = _mapper.Map<NhomZaloTask>(request);
                newNhomZaloTask.TrangThai = _config["Trangthai:Pending"];
                newNhomZaloTask.CreatedBy = currentUserId;
                newNhomZaloTask.CreatedTime = systemTimeNow;
                newNhomZaloTask.LastUpdatedTime = systemTimeNow;
                newNhomZaloTask.LastUpdatedBy = currentUserId;
                newNhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.AddAsync(newNhomZaloTask);
                
                // them user tu nhom zalo *CHUNG* vao usertask
                foreach (var userId in userNhomZalos)
                {
                    //var userTaskRequest = new CreateUserTaskCommand
                    var newUserTask = new UserTask
                    {
                        UserId = userId.UserId,
                        TaskId = request.TaskId,
                    };

                    //var newUserTask = _mapper.Map<UserTask>(userTaskRequest);
                    newUserTask.CreatedTime = systemTimeNow;
                    newUserTask.CreatedBy = currentUserId;
                    newUserTask.LastUpdatedTime = systemTimeNow;
                    newUserTask.LastUpdatedBy = currentUserId;
                    await _unitOfWork.UserTaskRepository.AddAsync(newUserTask);
                }
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<NhomZaloTaskReponse>(newNhomZaloTask);
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
