using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Queries
{
    public class GetRoleQuery : IRequest<IEnumerable<GetRoleResponse>>
    {
    }
}
