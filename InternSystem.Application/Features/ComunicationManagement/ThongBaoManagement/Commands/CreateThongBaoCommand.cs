
using FluentValidation;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands
{

    public class CreateThongBaoValidator : AbstractValidator<CreateThongBaoCommand>
    {
        public CreateThongBaoValidator()
        {
            RuleFor(m => m.IdNguoiNhan)
                .NotEmpty().WithMessage("Id người nhận không được để trống!");
            RuleFor(m => m.IdNguoiGui)
                .NotEmpty().WithMessage("Id người gửi không được để trống!");
            RuleFor(m => m.TieuDe)
                .NotEmpty().WithMessage("Tiêu đề không được để trống!");
            RuleFor(m => m.NoiDung)
                .NotEmpty().WithMessage("Nội dung không được để trống!");
            RuleFor(m => m.TinhTrang)
                .NotEmpty().WithMessage("Tình trạng không được để trống!");
        }
    }

    public class CreateThongBaoCommand : IRequest<CreateThongBaoResponse>
    {
        public string IdNguoiNhan { get; set; }

        public string IdNguoiGui { get; set; }

        public string TieuDe { get; set; }

        public string NoiDung { get; set; }

        public string TinhTrang { get; set; }

    }
}
