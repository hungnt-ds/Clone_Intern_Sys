using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands
{
    public class DeleteEmailUserStatusCommandValidator : AbstractValidator<DeleteEmailUserStatusCommand>
    {
        public DeleteEmailUserStatusCommandValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn EmailUserStatus để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteEmailUserStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
