using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands
{
    public class UpdateUserDuAnValidator : AbstractValidator<UpdateUserDuAnCommand>
    {
        public UpdateUserDuAnValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("User Id không được để trống!");
            RuleFor(m => m.DuAnId)
                .NotEmpty().WithMessage("Id của Dự Án không được để trống!");
            RuleFor(m => m.IdViTri)
                .NotEmpty().WithMessage("Id của Vị Trí không được để trống!");
        }
    }
    public class UpdateUserDuAnCommand : IRequest<UpdateUserDuAnResponse>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DuAnId { get; set; }
        public int IdViTri { get; set; }
    }
}
