using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries
{
    public class GetPagedCongNgheDuAnQuery : IRequest<PaginatedList<GetPagedCongNgheDuAnsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedCongNgheDuAnQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
