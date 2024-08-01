using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Queries
{
    public class GetPagedReportTasksQuery : IRequest<PaginatedList<GetPagedReportTasksResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedReportTasksQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
