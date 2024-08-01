using FluentValidation;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Queries
{
    public class GetEmailUserStatusByIdQueryValidator : AbstractValidator<GetEmailUserStatusByIdQuery>
    {
        public GetEmailUserStatusByIdQueryValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetEmailUserStatusByIdQuery : IRequest<GetDetailEmailUserStatusResponse>
    {
        public int Id { get; set; }
    }
}
