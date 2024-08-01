using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Commands
{
    public class DeleteLichPhongVanValidator : AbstractValidator<DeleteLichPhongVanCommand>
    {
        public DeleteLichPhongVanValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Lịch Phỏng Vấn để xóa.")
                .GreaterThan(0);
        }
    }


    public class DeleteLichPhongVanCommand : IRequest<bool>
    {
        public int Id { get; set; }

    }
}
