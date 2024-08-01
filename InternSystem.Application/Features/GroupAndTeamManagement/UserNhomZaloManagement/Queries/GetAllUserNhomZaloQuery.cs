using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Queries
{
    public class GetAllUserNhomZaloQuery : IRequest<IEnumerable<GetUserNhomZaloResponse>>
    {
    }
}
