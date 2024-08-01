using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries
{
    public class GetPagedDuAnQuery : IRequest<PaginatedList<GetPagedDuAnsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedDuAnQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
