using MediatR;

namespace InternSystem.Application.Common.Services.Interfaces
{
    public interface IMediatorService
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    }
}
