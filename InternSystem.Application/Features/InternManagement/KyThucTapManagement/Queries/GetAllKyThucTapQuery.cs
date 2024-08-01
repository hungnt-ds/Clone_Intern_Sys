using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries
{
    public class GetAllKyThucTapQuery : IRequest<IEnumerable<GetAllKyThucTapResponse>>
    {
    }
}
