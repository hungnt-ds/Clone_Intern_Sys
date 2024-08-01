using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using InternSystem.Domain.Entities;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries
{
    public class GetTruongHocByTenQuery : IRequest<IEnumerable<GetTruongHocByNameResponse>>
    {
        public string TenTruong { get; set; }

        public GetTruongHocByTenQuery(string ten)
        {
            TenTruong = ten;
        }
    }
}
