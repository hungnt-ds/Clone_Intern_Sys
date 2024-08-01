using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Commands
{
    public class DeleteInternInfoValidator : AbstractValidator<DeleteInternInfoCommand>
    {
        public DeleteInternInfoValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn InternInfo để xóa!")
                .GreaterThan(0);
        }
    }

    public class DeleteInternInfoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
