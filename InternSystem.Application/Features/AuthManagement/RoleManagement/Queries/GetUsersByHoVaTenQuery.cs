using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Domain.Entities;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Queries
{
    public class GetUsersByHoVaTenQuery : IRequest<IEnumerable<GetAllUserResponse>>
    {
        public string HoVaTen { get; set; }

        public GetUsersByHoVaTenQuery(string hoVaTen)
        {
            HoVaTen = hoVaTen;
        }
    }
}
