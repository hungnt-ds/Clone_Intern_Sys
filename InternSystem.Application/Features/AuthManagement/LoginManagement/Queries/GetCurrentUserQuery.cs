using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.LoginManagement.Queries
{
    public class GetCurrentUserQuery : IRequest<GetCurrentUserResponse>
    {
    }
}
