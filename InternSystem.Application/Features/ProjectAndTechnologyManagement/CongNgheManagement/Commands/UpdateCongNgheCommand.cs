using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands
{
    public class UpdateCongNgheValidator : AbstractValidator<UpdateCongNgheCommand>
    {
        public UpdateCongNgheValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
            RuleFor(m => m.Id)
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(m => m.Ten)
                .NotEmpty().WithMessage("Tên công nghệ không được để trống!");
            RuleFor(m => m.IdViTri)
                .NotEmpty().WithMessage("Id của vị trí không được để trống!");
        }
    }

    public class UpdateCongNgheCommand : IRequest<UpdateCongNgheResponse>
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public int IdViTri { get; set; }
        public string UrlImage { get; set; }
    }
}
