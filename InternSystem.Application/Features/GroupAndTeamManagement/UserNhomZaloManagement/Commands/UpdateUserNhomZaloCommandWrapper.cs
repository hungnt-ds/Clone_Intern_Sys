using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands
{
    public class UpdateUserNhomZaloCommandWrapper : IRequest<UpdateUserNhomZaloResponse>
    {
        public int Id { get; set; }
        public UpdateUserNhomZaloCommand Command { get; set; }
    }
}
