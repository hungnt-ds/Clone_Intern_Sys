using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries
{
    public class GetAllUserViTriQuery : IRequest<IEnumerable<GetAllUserViTriResponse>>
    {
    }
}
