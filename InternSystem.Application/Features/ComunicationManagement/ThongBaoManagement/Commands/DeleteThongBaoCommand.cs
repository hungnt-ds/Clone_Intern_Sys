using FluentValidation;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands
{
    public class DeleteThongBaoValidator : AbstractValidator<DeleteThongBaoCommand>
    {
        public DeleteThongBaoValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn thông báo để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteThongBaoCommand : IRequest<DeleteThongBaoResponse>
    {
        public int Id { get; set; }
    }
}
