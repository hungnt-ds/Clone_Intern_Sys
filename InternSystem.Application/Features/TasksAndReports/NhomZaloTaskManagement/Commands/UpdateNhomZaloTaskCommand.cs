using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands
{
    public class UpdateNhomZaloTaskValidator : AbstractValidator<UpdateNhomZaloTaskCommand>
    {
        public UpdateNhomZaloTaskValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Task của nhóm Zalo để Update!");
            RuleFor(m => m.TaskId)
                .NotEmpty().WithMessage("Chưa chọn Task!");
            RuleFor(m => m.NhomZaloId)
                .NotEmpty().WithMessage("Id nhóm Zalo không được để trống!");
            RuleFor(m => m.TrangThai)
                .NotEmpty().WithMessage("Trạng thái không được để trống!");

        }
    }

    public class UpdateNhomZaloTaskCommand : IRequest<NhomZaloTaskReponse>
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public int? NhomZaloId { get; set; }
        public string? TrangThai { get; set; }
    }
}
