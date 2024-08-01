using FluentValidation;
using MediatR;


namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands
{
    public class DeleteUserTaskValidator : AbstractValidator<DeleteUserTaskCommand>
    {
        public DeleteUserTaskValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn User để xóa Task!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteUserTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
