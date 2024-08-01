using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Queries
{
    public class GetUserNhomZaloByIdQuery : IRequest<GetUserNhomZaloResponse>
    {
        public int Id { get; }

        public GetUserNhomZaloByIdQuery(int id)
        {
            Id = id;
        }
    }
}
