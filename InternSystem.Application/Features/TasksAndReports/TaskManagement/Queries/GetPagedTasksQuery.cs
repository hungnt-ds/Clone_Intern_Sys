using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries
{
    public class GetPagedTasksQuery : IRequest<PaginatedList<GetPagedTasksResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedTasksQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
