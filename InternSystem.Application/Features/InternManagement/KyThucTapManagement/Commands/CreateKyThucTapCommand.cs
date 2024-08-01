using FluentValidation;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands
{

    public class CreateKyThucTapValidator : AbstractValidator<CreateKyThucTapCommand>
    {
        public CreateKyThucTapValidator()
        {
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên không được để trống!");
            RuleFor(m => m.NgayBatDau)
                .LessThan(m => m.NgayKetThuc).WithMessage("Ngày bắt đầu phải ngắn hơn ngày kết thúc.");
            RuleFor(m => m.NgayKetThuc)
                .GreaterThan(m => m.NgayBatDau).WithMessage("Ngày kết thúc phải dài hơn ngày bắt đầu.");
            RuleFor(m => m.IdTruong)
                .NotEmpty().WithMessage("Id Trường không được để trống!")
                .GreaterThan(0).WithMessage("Id Trường phải lớn hơn 0.");
        }
    }

    public class CreateKyThucTapCommand : IRequest<CreateKyThucTapResponse>
    {
        public string Ten { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int IdTruong { get; set; }

    }
}