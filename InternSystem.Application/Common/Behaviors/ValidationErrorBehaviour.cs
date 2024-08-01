using FluentValidation;
using MediatR;

namespace InternSystem.Application.Common.Behaviors
{
    public class ValidationErrorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationErrorBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);

            var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResult
                .Where(p => p.Errors.Any())
                .SelectMany(p => p.Errors)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
