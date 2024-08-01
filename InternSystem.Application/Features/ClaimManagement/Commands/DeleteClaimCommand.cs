using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ClaimManagement.Commands
{
    public class DeleteClaimCommandValidator : AbstractValidator<DeleteClaimCommand>
    {
        public DeleteClaimCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Chưa chọn Claim để xóa!");
        }
    }

    public class DeleteClaimCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
