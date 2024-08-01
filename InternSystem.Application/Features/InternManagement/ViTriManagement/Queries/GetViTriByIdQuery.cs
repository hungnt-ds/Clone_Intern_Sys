using FluentValidation;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Queries
{
    public class GetViTriByIdValidator : AbstractValidator<GetViTriByIdQuery>
    {
        public GetViTriByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetViTriByIdQuery : IRequest<GetViTriByIdResponse>
    {
        public int Id { get; set; }
    }
}
