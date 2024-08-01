using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Queries
{
    public class GetPagedThongBaosQuery : IRequest<PaginatedList<GetPagedThongBaosResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedThongBaosQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
