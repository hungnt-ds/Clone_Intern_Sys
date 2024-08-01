using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Queries
{
    public class GetAllViTriQuery : IRequest<IEnumerable<GetAllViTriResponse>>
    {
    }
}
