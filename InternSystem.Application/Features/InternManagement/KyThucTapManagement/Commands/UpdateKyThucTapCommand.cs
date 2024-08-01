using FluentValidation;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands
{

    public class UpdateKyThucTapValidator : AbstractValidator<UpdateKyThucTapCommand>
    {
        public UpdateKyThucTapValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class UpdateKyThucTapCommand : IRequest<UpdateKyThucTapResponse>
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int? IdTruong { get; set; }
    }
}
