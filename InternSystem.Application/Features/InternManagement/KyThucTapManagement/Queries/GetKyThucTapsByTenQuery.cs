using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries
{
    public class GetKyThucTapsByTenQuery : IRequest<IEnumerable<GetKyThucTapsByNameResponse>>
    {
        public string Ten { get; set; }

        public GetKyThucTapsByTenQuery(string ten)
        {
            Ten = ten;
        }
    }
}

