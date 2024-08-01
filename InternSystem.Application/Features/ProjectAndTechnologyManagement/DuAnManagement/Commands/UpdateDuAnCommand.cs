using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands
{
    public class UpdateDuAnValidator : AbstractValidator<UpdateDuAnCommand>
    {
        public UpdateDuAnValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên dự án không được để trống!");
            RuleFor(m => m.ThoiGianBatDau)
                .LessThan(m => m.ThoiGianKetThuc).WithMessage("Thời gian bắt đầu phải ngắn hơn thời gian kết thúc.");
            RuleFor(m => m.ThoiGianKetThuc)
                .GreaterThan(m => m.ThoiGianBatDau).WithMessage("Thời gian kết thúc phải dài hơn thời gian bắt đầu.");
        }
    }


    public class UpdateDuAnCommand : IRequest<UpdateDuAnResponse>
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public string? LeaderId { get; set; }
        public DateTimeOffset? ThoiGianBatDau { get; set; }
        public DateTimeOffset? ThoiGianKetThuc { get; set; }
    }
}
