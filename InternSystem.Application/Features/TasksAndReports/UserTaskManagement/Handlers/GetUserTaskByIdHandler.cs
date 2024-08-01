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
    public class GetUserTaskByIdHandler : IRequestHandler<GetUserTaskByIdQuery, UserTaskReponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserTaskByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserTaskReponse> Handle(GetUserTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //UserTask? existingUserTask = await _unitOfWork.UserTaskRepository.GetByIdAsync(request.Id);
                //if (existingUserTask == null || existingUserTask.IsDelete == true)
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng thực hiện task");

                //return _mapper.Map<UserTaskReponse>(existingUserTask);
                var repository = _unitOfWork.GetRepository<UserTask>();
                var userRepository = _unitOfWork.UserRepository;

                var userTaskById = await repository
                    .GetAllQueryable()
                    .FirstOrDefaultAsync(ut => ut.Id == request.Id && !ut.IsDelete);

                if(userTaskById == null || userTaskById.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng của công việc");
                }

                var response = _mapper.Map<UserTaskReponse>(userTaskById);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";

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
