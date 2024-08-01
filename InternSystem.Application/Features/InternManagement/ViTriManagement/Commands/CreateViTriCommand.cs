using FluentValidation;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Commands
{
    public class CreateViTriValidator : AbstractValidator<CreateViTriCommand>
    {
        public CreateViTriValidator()
        {
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên không được để trống!");
            RuleFor(m => m.DuAnId)
                .NotEmpty().WithMessage("Id Dự Án không được để trống!")
                .GreaterThan(0).WithMessage("Id Dự án phải lớn hơn 0.");
        }
    }
    public class CreateViTriCommand : IRequest<CreateViTriResponse>
    {
        public string? Ten { get; set; }
        public string? LinkNhomZalo { get; set; }
        public int DuAnId { get; set; }
    }
}
