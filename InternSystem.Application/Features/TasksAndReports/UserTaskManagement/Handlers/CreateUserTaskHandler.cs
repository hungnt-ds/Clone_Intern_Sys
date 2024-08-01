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
    public class CreateUserTaskHandler : IRequestHandler<CreateUserTaskCommand, UserTaskReponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateUserTaskHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UserTaskReponse> Handle(CreateUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                AspNetUser? existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (existingUser == null || existingUser.IsDelete == true)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }

                Tasks? existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
                if (existingTask == null || existingTask.IsDelete == true || existingTask.HoanThanh == true)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task");
                }

                // Check for the exsisting of User - Task
                IEnumerable<UserTask>? allUserTasks = await _unitOfWork.UserTaskRepository.GetAllAsync();
                List<UserTask>? userTaskList = allUserTasks.ToList();
                foreach (var userTask in userTaskList)
                {
                    if (userTask.TaskId == request.TaskId && userTask.UserId == request.UserId)
                    {
                        throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Người dùng đã thực hiện task");
                    }
                }

                var newUserTask = _mapper.Map<UserTask>(request);
                newUserTask.TrangThai = _config["Trangthai:Pending"];
                newUserTask.CreatedTime = _timeService.SystemTimeNow;
                newUserTask.CreatedBy = currentUserId;
                newUserTask.LastUpdatedTime = _timeService.SystemTimeNow;
                newUserTask.LastUpdatedBy = currentUserId;
                newUserTask = await _unitOfWork.UserTaskRepository.AddAsync(newUserTask);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UserTaskReponse>(newUserTask);
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
