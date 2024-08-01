using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands
{
    public class UpdateNhomZaloCommandWrapper : IRequest<UpdateNhomZaloResponse>
    {
        public int Id { get; set; }
        public UpdateNhomZaloCommand Command { get; set; }
    }
}
