using FluentValidation;
using MediatR;


namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Commands
{
    public class DeleteTaskValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Task để xóa!")
                .GreaterThan(0);
        }
    }

    public class DeleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
