using InternSystem.Application.Features.ClaimManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ClaimManagement.Queries
{
    public class GetAllClaimQuery : IRequest<IEnumerable<GetClaimResponse>>
    {
    }
}
