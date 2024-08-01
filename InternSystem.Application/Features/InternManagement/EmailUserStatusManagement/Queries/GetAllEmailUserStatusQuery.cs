using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Queries
{
    public class GetAllEmailUserStatusQuery : IRequest<IEnumerable<GetDetailEmailUserStatusResponse>>
    {
    }
}
