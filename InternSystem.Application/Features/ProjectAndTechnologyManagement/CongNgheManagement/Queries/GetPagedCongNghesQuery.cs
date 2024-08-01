using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CongNgheManagement.Queries
{
    public class GetPagedCongNghesQuery : IRequest<PaginatedList<GetPagedCongNghesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedCongNghesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
