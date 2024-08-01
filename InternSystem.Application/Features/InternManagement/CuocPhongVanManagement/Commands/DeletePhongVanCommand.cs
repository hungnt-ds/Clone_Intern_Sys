using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Commands
{
    public class DeletePhongVanValidator : AbstractValidator<DeletePhongVanCommand>
    {
        public DeletePhongVanValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn cuộc phỏng vấn để xóa.")
                .GreaterThan(0).WithMessage("Id phỏng vấn phải lớn hơn 0.");
        }
    }
    public class DeletePhongVanCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
