using FluentValidation;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Commands
{
    public class UpdateViTriValidator : AbstractValidator<UpdateViTriCommand>
    {
        public UpdateViTriValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Vị trí để Update!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(m => m.DuAnId)
                .NotEmpty().WithMessage("Id Dự Án không được để trống!");
        }
    }
    public class UpdateViTriCommand : IRequest<UpdateViTriResponse>
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public string? LinkNhomZalo { get; set; }
        public int DuAnId { get; set; }
    }
}
