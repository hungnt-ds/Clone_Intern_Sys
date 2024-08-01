using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Commands
{
    public class AddRoleCommandValidation : AbstractValidator<AddRoleCommand>
    {
        public AddRoleCommandValidation()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Tên role không được để trống!");
        }
    }
    public class AddRoleCommand : IRequest<bool>
    {
        public string? Name { get; set; }
    }
}

