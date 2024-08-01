using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands
{
    public class UpdateTaskReportValidator : AbstractValidator<UpdateTaskReportCommand>
    {
        public UpdateTaskReportValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Task Report để Update!");
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("Chưa có UserId!");
            RuleFor(m => m.TaskId)
                .NotEmpty().WithMessage("Chưa chọn Task!");
            RuleFor(m => m.MoTa)
                .NotEmpty().WithMessage("Mô tả không được để trống!");
            RuleFor(m => m.NoiDungBaoCao)
                .NotEmpty().WithMessage("Nội dung báo cáo không được để trống!");
        }
    }

    public class UpdateTaskReportCommand : IRequest<TaskReportResponse>
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? TaskId { get; set; }
        public string? MoTa { get; set; }
        public string? NoiDungBaoCao { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? NgayBaoCao { get; set; } = DateTime.Now;
    }
}
