using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries
{
    public class GetAllCongNgheDuAnQuery : IRequest<IEnumerable<GetAllCongNgheDuAnResponse>>
    {
    }
}
