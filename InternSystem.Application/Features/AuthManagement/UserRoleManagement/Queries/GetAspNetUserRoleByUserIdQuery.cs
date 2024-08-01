using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Queries
{
    public class GetAspNetUserRoleByUserIdQuery : IRequest<List<string>>
    {
        public string UserId { get; set; }
    }
}
