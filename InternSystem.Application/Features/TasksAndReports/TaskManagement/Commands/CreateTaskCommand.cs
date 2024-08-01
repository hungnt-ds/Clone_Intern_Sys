using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Commands
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskValidator()
        {
            RuleFor(m => m.DuAnId)
                .NotEmpty().WithMessage("Chưa chọn Dự án!");
            RuleFor(m => m.MoTa)
                .NotEmpty().WithMessage("Mô tả không được để trống!");
            RuleFor(m => m.NoiDung)
                .NotEmpty().WithMessage("Nội dung không được để trống!");
            RuleFor(m => m.NgayGiao)
                .LessThan(m => m.HanHoanThanh).WithMessage("Ngày giao phải ngắn hơn ngày hoàn thành.");
        }
    }

    public class CreateTaskCommand : IRequest<TaskResponse>
    {
        public int DuAnId { get; set; }
        public string MoTa { get; set; }
        public string NoiDung { get; set; }
        public DateTime? NgayGiao { get; set; }
        public DateTime? HanHoanThanh { get; set; }
    }
}
