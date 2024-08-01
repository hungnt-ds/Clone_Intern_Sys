using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands
{
    public class CreateUserDuAnValidator : AbstractValidator<CreateUserDuAnCommand>
    {
        public CreateUserDuAnValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("User Id không được để trống!");
            RuleFor(m => m.DuAnId)
                .NotEmpty().WithMessage("Id của Dự Án không được để trống!");
            RuleFor(m => m.IdViTri)
                .NotEmpty().WithMessage("Id của Vị Trí không được để trống!");
        }
    }
    public class CreateUserDuAnCommand : IRequest<CreateUserDuAnResponse>
    {
        public string UserId { get; set; }
        public int DuAnId { get; set; }
        public int IdViTri { get; set; }
    }
}
