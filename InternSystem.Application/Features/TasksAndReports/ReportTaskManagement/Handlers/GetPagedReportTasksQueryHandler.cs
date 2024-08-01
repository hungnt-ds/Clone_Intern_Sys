using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
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
    public class GetPagedReportTasksQueryHandler : IRequestHandler<GetPagedReportTasksQuery, PaginatedList<GetPagedReportTasksResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedReportTasksQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedReportTasksResponse>> Handle(GetPagedReportTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<ReportTask>();
                var userRepository = _unitOfWork.UserRepository;

                var query = repository.GetAllQueryable()
                    .Include(rt => rt.User)
                    .Where(rt => rt.IsActive && !rt.IsDelete);

                var paginatedItems = await PaginatedList<ReportTask>.CreateAsync(
                    query,
                    request.PageNumber,
                    request.PageSize
             
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedReportTasksResponse>(item)).ToList();

                foreach (var reportTaskResponse in responseItems)
                {
                    reportTaskResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(reportTaskResponse.CreatedBy) ?? "Người dùng không xác định";
                    reportTaskResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(reportTaskResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedReportTasksResponse>(
                    responseItems,
                    paginatedItems.TotalCount,
                    paginatedItems.PageNumber,
                    paginatedItems.PageSize
                );

                return responsePaginatedList;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }

    }
}
