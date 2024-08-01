using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Queries
{
    public class GetAllNhomZaloQuery : IRequest<IEnumerable<GetNhomZaloResponse>>
    {
    }
}
