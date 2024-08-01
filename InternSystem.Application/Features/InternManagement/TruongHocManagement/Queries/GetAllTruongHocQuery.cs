using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries
{
    public class GetAllTruongHocQuery : IRequest<IEnumerable<GetAllTruongHocResponse>>
    {
    }
}
