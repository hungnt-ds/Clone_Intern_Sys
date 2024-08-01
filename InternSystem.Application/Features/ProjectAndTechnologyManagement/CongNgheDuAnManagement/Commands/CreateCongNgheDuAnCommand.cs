using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands
{
    public class CreateCongNgheDuAnValidator : AbstractValidator<CreateCongNgheDuAnCommand>
    {
        public CreateCongNgheDuAnValidator()
        {
            RuleFor(m => m.IdCongNghe)
                .NotEmpty().WithMessage("Id của công nghệ không được để trống!");
            RuleFor(m => m.IdDuAn)
                .NotEmpty().WithMessage("Id của dự án không được để trống!");
        }
    }
    public class CreateCongNgheDuAnCommand : IRequest<CreateCongNgheDuAnResponse>
    {
        public int IdCongNghe { get; set; }
        public int IdDuAn { get; set; }

    }
}
