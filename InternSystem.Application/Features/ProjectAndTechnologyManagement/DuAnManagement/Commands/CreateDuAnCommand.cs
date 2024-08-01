using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands
{
    public class CreateDuAnValidator : AbstractValidator<CreateDuAnCommand>
    {
        public CreateDuAnValidator()
        {
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên dự án không được để trống!");
            RuleFor(m => m.ThoiGianBatDau)
                .LessThan(m => m.ThoiGianKetThuc).WithMessage("Thời gian bắt đầu phải ngắn hơn thời gian kết thúc.");
            RuleFor(m => m.ThoiGianKetThuc)
                .GreaterThan(m => m.ThoiGianBatDau).WithMessage("Thời gian kết thúc phải dài hơn thời gian bắt đầu.");
        }
    }

    public class CreateDuAnCommand : IRequest<CreateDuAnResponse>
    {
        public string Ten { get; set; }
        public string? LeaderId { get; set; }
        public DateTimeOffset ThoiGianBatDau { get; set; }
        public DateTimeOffset ThoiGianKetThuc { get; set; }
    }
}
