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
    public class GetTaskHandler : IRequestHandler<GetAllTaskQuery, IEnumerable<TaskResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTaskHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetAllTaskQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskRepository = _unitOfWork.GetRepository<Tasks>();
                var userRepository = _unitOfWork.UserRepository;

                var tasks = await taskRepository.GetAllQueryable()
                    .Include(task => task.DuAn)
                    .Where(task => task.IsActive && !task.IsDelete)
                    .ToListAsync(cancellationToken);

                if (!tasks.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy task"
                    );
                }

                var response = _mapper.Map<IEnumerable<TaskResponse>>(tasks);

                foreach (var item in response)
                {
                    item.CreatedName = await userRepository.GetUserNameByIdAsync(item.CreatedBy) ?? "Người dùng không xác định";
                    item.LastUpdatedName = await userRepository.GetUserNameByIdAsync(item.LastUpdatedBy) ?? "Người dùng không xác định";
                }
                return response;
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
