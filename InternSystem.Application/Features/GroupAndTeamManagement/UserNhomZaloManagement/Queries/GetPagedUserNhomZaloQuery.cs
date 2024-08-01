using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Queries
{
    public class GetPagedUserNhomZaloQuery : IRequest<PaginatedList<GetPagedUserNhomZaloResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedUserNhomZaloQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}

