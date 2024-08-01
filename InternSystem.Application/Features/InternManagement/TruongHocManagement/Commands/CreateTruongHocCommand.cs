using FluentValidation;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands
{
    public class CreateTruongHocValidator : AbstractValidator<CreateTruongHocCommand>
    {
        public CreateTruongHocValidator()
        {
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên Trường không được để trống!");
            RuleFor(m => m.SoTuanThucTap)
                .GreaterThan(0).WithMessage("Số tuần thực tập phải lớn hơn 0.")
                .LessThanOrEqualTo(14).WithMessage("Số tuàn thực tập không thể vượt quá 14 tuần.");
        }
    }

    public class CreateTruongHocCommand : IRequest<CreateTruongHocResponse>
    {
        public string Ten { get; set; }
        public int SoTuanThucTap { get; set; }
    }
}
