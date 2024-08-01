using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands
{
    public class CreateTaskReportValidator : AbstractValidator<CreateTaskReportCommand>
    {
        public CreateTaskReportValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("User Id không được để trống!");
            RuleFor(m => m.TaskId)
                .NotEmpty().WithMessage("Chưa chọn Task để tạo Report!");
            RuleFor(m => m.MoTa)
                .NotEmpty().WithMessage("Mô tả không được để trống!");
            RuleFor(m => m.NoiDungBaoCao)
                .NotEmpty().WithMessage("Nội dung báo cáo không được để trống!");
        }
    }

    public class CreateTaskReportCommand : IRequest<TaskReportResponse>
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public string MoTa { get; set; }
        public string NoiDungBaoCao { get; set; }
        public DateTime NgayBaoCao { get; set; }
    }
}
