using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands
{
    public class CreateNhomZaloTaskValidator : AbstractValidator<CreateNhomZaloTaskCommand>
    {
        public CreateNhomZaloTaskValidator()
        {
            RuleFor(m => m.TaskId)
                .NotEmpty().WithMessage("Task Id không được để trống!");
            RuleFor(m => m.NhomZaloId)
                .NotEmpty().WithMessage("Id nhóm Zalo không được để trống!");
        }
    }

    public class CreateNhomZaloTaskCommand : IRequest<NhomZaloTaskReponse>
    {
        public int TaskId { get; set; }
        public int NhomZaloId { get; set; }

    }
}
