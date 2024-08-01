using FluentValidation;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands
{
    public class UpdateTruongHocValidator : AbstractValidator<UpdateTruongHocCommand>
    {
        public UpdateTruongHocValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Trường Học để Update!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên không được để trống.");
            RuleFor(m => m.SoTuanThucTap)
                .GreaterThan(0).WithMessage("Số tuần thực tập phải lớn hơn 0.");
        }
    }

    public class UpdateTruongHocCommand : IRequest<UpdateTruongHocResponse>
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public int? SoTuanThucTap { get; set; }
    }
}
