using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries
{
    public class GetPagedUserViTriQuery : IRequest<PaginatedList<GetPagedUserViTriResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedUserViTriQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
