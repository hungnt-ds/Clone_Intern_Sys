using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Handlers
{
    public class GetTaskReportByIdHandler : IRequestHandler<GetTaskReportByIdQuery, GetReportByIDReponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTaskReportByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetReportByIDReponse> Handle(GetTaskReportByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reportTaskRepository = _unitOfWork.GetRepository<ReportTask>();
                var userRepository = _unitOfWork.UserRepository;

                var existingReportTask = await reportTaskRepository
                    .GetAllQueryable() 
                    .Include(rt => rt.User)   
                    .FirstOrDefaultAsync(rt => rt.Id == request.Id && !rt.IsDelete, cancellationToken);

                if (existingReportTask == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task report");
                }

                var response = _mapper.Map<GetReportByIDReponse>(existingReportTask);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";
                
                return response;
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lấy dữ liệu.");
            }
        }

    }
}
