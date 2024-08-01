using InternSystem.Application.Common.Services.Interfaces;
using MediatR;

namespace InternSystem.Infrastructure.Services
{
    public class MediatorService : IMediatorService
    {
        private readonly IMediator _mediator;

        public MediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            return await _mediator.Send(request);
        }
    }
}
