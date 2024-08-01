using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CauhoiManagement.Queries
{
    public class GetPagedCauHoisQuery : IRequest<PaginatedList<GetPagedCauHoisResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedCauHoisQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
