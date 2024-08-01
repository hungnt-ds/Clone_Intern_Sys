using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Queries
{
    public class GetNhomZaloByIdQuery : IRequest<GetNhomZaloResponse>
    {
        public int Id { get; set; }

        public GetNhomZaloByIdQuery(int id)
        {
            Id = id;
        }
    }
}
