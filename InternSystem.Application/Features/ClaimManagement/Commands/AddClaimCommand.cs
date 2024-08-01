using FluentValidation;
using InternSystem.Application.Features.ClaimManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ClaimManagement.Commands
{
    public class CreateAddClaimValidator : AbstractValidator<AddClaimCommand>
    {
        public CreateAddClaimValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type không được để trống!");
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value không được để trống!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Desciption không được để trống!");
        }
    }
    public class AddClaimCommand : IRequest<GetClaimResponse>
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
