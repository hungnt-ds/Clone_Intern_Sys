using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetInternInfoByTruongHocNameQuery : IRequest<IEnumerable<GetInternInfoResponse>>
    {
        public string TruongHocName { get; set; }

        public GetInternInfoByTruongHocNameQuery() { }

        public GetInternInfoByTruongHocNameQuery(string truongHocName)
        {
            TruongHocName = truongHocName;
        }
    }
}
