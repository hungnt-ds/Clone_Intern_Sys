using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands
{
    public class CreateCongNgheValidator : AbstractValidator<CreateCongNgheCommand>
    {
        public CreateCongNgheValidator()
        {
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên công nghệ không được để trống!");
            RuleFor(m => m.IdViTri)
                .NotEmpty().WithMessage("Id không được để trống!");
        }
    }
    public class CreateCongNgheCommand : IRequest<CreateCongNgheResponse>
    {
        public string Ten { get; set; }
        public int IdViTri { get; set; }
        public string UrlImage { get; set; }
    }
}
