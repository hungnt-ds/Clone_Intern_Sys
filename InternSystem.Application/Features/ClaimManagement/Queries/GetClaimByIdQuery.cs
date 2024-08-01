using FluentValidation;
using InternSystem.Application.Features.ClaimManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ClaimManagement.Queries
{

    public class GetClaimByIdValidator : AbstractValidator<GetClaimByIdQuery>
    {
        public GetClaimByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .GreaterThan(0);
        }
    }
    public class GetClaimByIdQuery : IRequest<GetClaimResponse>
    {
        public GetClaimByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
