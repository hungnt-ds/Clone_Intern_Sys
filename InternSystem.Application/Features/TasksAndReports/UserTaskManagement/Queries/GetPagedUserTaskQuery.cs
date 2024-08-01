using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Queries
{
    public class GetPagedUserTaskQuery : IRequest<PaginatedList<GetPagedUserTaskResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedUserTaskQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
