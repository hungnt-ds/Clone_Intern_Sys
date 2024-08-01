using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Queries
{
    public class GetPagedRolesQuery : IRequest<PaginatedList<GetPagedRolesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedRolesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
