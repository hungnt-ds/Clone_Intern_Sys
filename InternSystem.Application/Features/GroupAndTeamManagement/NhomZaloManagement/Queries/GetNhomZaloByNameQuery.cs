using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Queries
{
    public class GetNhomZaloByNameQuery : IRequest<GetNhomZaloResponse>
    {
        public string TenNhom { get; set; }

        public GetNhomZaloByNameQuery(string name)
        {
            TenNhom = name;
        }
    }
}
