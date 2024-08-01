using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Queries
{
    public class GetAllCongNgheQuery : IRequest<IEnumerable<GetAllCongNgheResponse>>
    {
    }
}
