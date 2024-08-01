using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries
{
    public class GetPagedTruongHocsQuery : IRequest<PaginatedList<GetPagedTruongHocsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedTruongHocsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
