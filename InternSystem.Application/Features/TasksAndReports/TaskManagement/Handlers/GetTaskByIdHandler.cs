using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Handlers
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTaskByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaskResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskRepository = _unitOfWork.GetRepository<Tasks>();
                var userRepository = _unitOfWork.UserRepository;

                var task = await taskRepository.GetAllQueryable()
                    .Include(t => t.DuAn)
                    .FirstOrDefaultAsync(t => t.Id == request.Id && !t.IsDelete, cancellationToken);

                if (task == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy task"
                    );
                }

                var taskResponse = _mapper.Map<TaskResponse>(task);

                taskResponse.TenDuAn = task.DuAn.Ten;
                taskResponse.CreatedBy = await userRepository.GetUserNameByIdAsync(task.CreatedBy) ?? "Người dùng không xác định";
                taskResponse.LastUpdatedBy = await userRepository.GetUserNameByIdAsync(task.LastUpdatedBy) ?? "Người dùng không xác định";
                
                return taskResponse;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(
                    StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    "Đã xảy ra lỗi không mong muốn khi lấy dữ liệu."
                );
            }
        }

    }
}
