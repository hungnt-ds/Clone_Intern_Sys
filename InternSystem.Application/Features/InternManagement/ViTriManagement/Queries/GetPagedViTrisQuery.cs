using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Queries
{
    public class GetPagedViTrisQuery : IRequest<PaginatedList<GetPagedViTrisResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedViTrisQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
