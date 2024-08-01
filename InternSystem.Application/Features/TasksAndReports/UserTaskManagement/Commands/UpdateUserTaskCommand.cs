using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;


namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands
{
    public class UpdateUserTaskValidator : AbstractValidator<UpdateUserTaskCommand>
    {
        public UpdateUserTaskValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn UserTask để Update!");
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("Chưa chọn User!");
            RuleFor(m => m.TaskId)
                .NotEmpty().WithMessage("Chưa chọn Task!");
            RuleFor(m => m.TrangThai)
                .NotEmpty().WithMessage("Chưa cập nhật Trạng thái.");
        }
    }

    public class UpdateUserTaskCommand : IRequest<UserTaskReponse>
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? TaskId { get; set; }
        public string? TrangThai { get; set; }
    }
}
