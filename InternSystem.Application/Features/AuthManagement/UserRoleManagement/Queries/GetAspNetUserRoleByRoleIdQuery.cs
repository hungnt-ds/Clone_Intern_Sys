using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Queries
{
    public class GetAspNetUserRoleByRoleIdQuery : IRequest<List<GetAspNetUserResponse>>
    {
        public string RoleId { get; set; }
    }
}
