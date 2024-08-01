using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Queries
{
    public class GetPagedPhongVanQuery : IRequest<PaginatedList<GetPagedPhongVansResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedPhongVanQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
