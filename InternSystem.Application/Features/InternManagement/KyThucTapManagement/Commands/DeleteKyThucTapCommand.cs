using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands
{
    public class DeleteKyThucTapValidator : AbstractValidator<DeleteKyThucTapCommand>
    {
        public DeleteKyThucTapValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteKyThucTapCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
