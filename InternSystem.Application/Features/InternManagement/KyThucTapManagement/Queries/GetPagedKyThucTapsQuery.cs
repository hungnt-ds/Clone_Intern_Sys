using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries
{
    public class GetPagedKyThucTapsQuery : IRequest<PaginatedList<GetPagedKyThucTapsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedKyThucTapsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
