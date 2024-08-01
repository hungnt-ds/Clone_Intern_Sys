using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Queries
{
    public class GetAllPhongVanQuery : IRequest<IEnumerable<GetAllPhongVanResponse>>
    {
    }
}

