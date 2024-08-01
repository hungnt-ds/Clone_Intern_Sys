using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Queries
{
    public class GetPagedCauHoiCongNgheQuery : IRequest<PaginatedList<GetPagedCauHoiCongNghesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedCauHoiCongNgheQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
