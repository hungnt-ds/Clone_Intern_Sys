using FluentValidation;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands
{
    public class UpdateThongBaoValidator : AbstractValidator<UpdateThongBaoCommand>
    {
        public UpdateThongBaoValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn thông báo để Update!")
                .GreaterThan(0);
        }
    }

    public class UpdateThongBaoCommand : IRequest<UpdateThongBaoResponse>
    {
        public int Id { get; set; }

        public string? IdNguoiNhan { get; set; }

        public string? IdNguoiGui { get; set; }

        public string? TieuDe { get; set; }

        public string? NoiDung { get; set; }

        public string? TinhTrang { get; set; }


    }
}
