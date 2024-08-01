using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Handlers
{
    public class GetUserTaskHandler : IRequestHandler<GetUserTaskQuery, IEnumerable<UserTaskReponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserTaskHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserTaskReponse>> Handle(GetUserTaskQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //IEnumerable<UserTask> userTasks = await _unitOfWork.UserTaskRepository.GetUserTasksAsync();
                //return _mapper.Map<IEnumerable<UserTaskReponse>>(userTasks);
                var userTaskRepository = _unitOfWork.GetRepository<UserTask>();
                var userRepository = _unitOfWork.UserRepository;

                var listUserTask = await userTaskRepository
                    .GetAllQueryable()
                    .Where(ut => ut.IsActive && !ut.IsDelete)
                    .ToListAsync(cancellationToken);

                if(listUserTask == null || !listUserTask.Any())
                {
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng của công việc.");
                }

                var response = _mapper.Map<IEnumerable<UserTaskReponse>>(listUserTask);

                foreach (var userTaskReponse in response)
                {
                    userTaskReponse.CreatedByName = await userRepository.GetUserNameByIdAsync(userTaskReponse.CreatedBy) ?? "Người dùng không xác định";
                    userTaskReponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(userTaskReponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                return response;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lấy dữ liệu.");
            }
        }
    }
}
