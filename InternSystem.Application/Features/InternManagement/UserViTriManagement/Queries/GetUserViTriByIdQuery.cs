using FluentValidation;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries
{
    public class GetUserViTriByIdValidator : AbstractValidator<GetUserViTriByIdQuery>
    {
        public GetUserViTriByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetUserViTriByIdQuery : IRequest<GetUserViTriByIdResponse>
    {
        public int Id { get; set; }
    }
}
