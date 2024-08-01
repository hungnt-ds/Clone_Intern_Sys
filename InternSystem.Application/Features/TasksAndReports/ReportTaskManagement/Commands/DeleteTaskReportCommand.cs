using FluentValidation;
using MediatR;


namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands
{
    public class DeleteTaskReportValidator : AbstractValidator<DeleteTaskReportCommand>
    {
        public DeleteTaskReportValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Task Report để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteTaskReportCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
