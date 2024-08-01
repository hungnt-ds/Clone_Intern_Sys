using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries
{
    public class GetAllDuAnQuery : IRequest<IEnumerable<GetAllDuAnResponse>>
    {
    }
}
