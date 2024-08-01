using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Queries
{
    public class GetUserByIdQuery : IRequest<GetUserDetailResponse>
    {
        public string UserId { get; set; }
        public GetUserByIdQuery(string userid)
        {
            UserId = userid;
        }
    }
}
