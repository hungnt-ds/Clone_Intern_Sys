using FluentValidation;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetInternInfoByIdValidator : AbstractValidator<GetInternInfoByIdQuery>
    {
        public GetInternInfoByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetInternInfoByIdQuery : IRequest<GetInternInfoByIdResponse>
    {
        public int Id { get; set; }
    }
}
