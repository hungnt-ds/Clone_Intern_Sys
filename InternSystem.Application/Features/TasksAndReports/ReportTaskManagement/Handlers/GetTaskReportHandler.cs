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
    public class GetTaskReportHandler : IRequestHandler<GetTaskReportQuery, IEnumerable<GetReportAllReponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetTaskReportHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetReportAllReponse>> Handle(GetTaskReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reportTaskRepository = _unitOfWork.GetRepository<ReportTask>();
                var userRepository = _unitOfWork.UserRepository;

                var existingReportTasks = await reportTaskRepository
                    .GetAllQueryable() 
                    .Include(rt => rt.User)
                    .Where(da => da.IsActive && !da.IsDelete)
                    .ToListAsync(cancellationToken);

                if (existingReportTasks == null || !existingReportTasks.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy task report");
                }

                var response = _mapper.Map<IEnumerable<GetReportAllReponse>>(existingReportTasks);
                foreach (var reportResponse in response)
                {
                    reportResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(reportResponse.CreatedBy) ?? "Người dùng không xác định";
                    reportResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(reportResponse.LastUpdatedBy) ?? "Người dùng không xác định";
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
