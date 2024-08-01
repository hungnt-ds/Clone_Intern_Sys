using MediatR;

namespace InternSystem.Application.Features.BaseFeature.Queries
{
    public class GetAllQuery<TResponse> : IRequest<IEnumerable<TResponse>>
    {
    }
}
