using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries
{
    public class GetAllUserDuAnQuery : IRequest<IEnumerable<GetAllUserDuAnResponse>>
    {
    }
}
