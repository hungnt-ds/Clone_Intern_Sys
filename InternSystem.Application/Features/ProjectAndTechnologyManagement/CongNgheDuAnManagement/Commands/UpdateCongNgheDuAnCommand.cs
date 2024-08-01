using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands
{
    public class UpdateCongNgheDuAnValidator : AbstractValidator<UpdateCongNgheDuAnCommand>
    {
        public UpdateCongNgheDuAnValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
            RuleFor(m => m.IdCongNghe)
                .NotEmpty().WithMessage("Id của công nghệ không được để trống!");
            RuleFor(m => m.IdDuAn)
                .NotEmpty().WithMessage("Id của dự án không được để trống!");
        }
    }


    public class UpdateCongNgheDuAnCommand : IRequest<UpdateCongNgheDuAnResponse>
    {
        public int Id { get; set; }
        public int IdCongNghe { get; set; }
        public int IdDuAn { get; set; }
    }
}
