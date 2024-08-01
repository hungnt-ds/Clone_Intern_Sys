using FluentValidation;
using InternSystem.Application.Features.ClaimManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ClaimManagement.Commands
{
    public class UpdateClaimCommandValidator : AbstractValidator<UpdateClaimCommand>
    {
        public UpdateClaimCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Chưa chọn Claim để Update!");
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type không được để trống!");
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value không được để trống!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Desciption không được để trống!");
        }
    }

    public class UpdateClaimCommand : IRequest<GetClaimResponse>
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
