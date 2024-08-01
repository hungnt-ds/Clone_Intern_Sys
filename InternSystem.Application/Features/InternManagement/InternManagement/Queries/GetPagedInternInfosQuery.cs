using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetPagedInternInfosQuery : IRequest<PaginatedList<GetPagedInternInfosResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedInternInfosQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
