using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands
{
    public class CreateUserTaskValidator : AbstractValidator<CreateUserTaskCommand>
    {
        public CreateUserTaskValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("Chưa chọn User để giao Task!");
            RuleFor(m => m.TaskId)
                .NotEmpty().WithMessage("Task Id không được để trống!");
        }
    }

    public class CreateUserTaskCommand : IRequest<UserTaskReponse>
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
    }
}
