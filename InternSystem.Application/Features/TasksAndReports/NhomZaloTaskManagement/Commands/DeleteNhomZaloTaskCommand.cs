using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands
{
    public class DeleteNhomZaloTaskValidator : AbstractValidator<DeleteNhomZaloTaskCommand>
    {
        public DeleteNhomZaloTaskValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Task của nhóm Zalo để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteNhomZaloTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
