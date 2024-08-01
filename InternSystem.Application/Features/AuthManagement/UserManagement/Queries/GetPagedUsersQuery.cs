using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Queries
{
    public class GetPagedUsersQuery : IRequest<PaginatedList<GetPagedUsersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedUsersQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
