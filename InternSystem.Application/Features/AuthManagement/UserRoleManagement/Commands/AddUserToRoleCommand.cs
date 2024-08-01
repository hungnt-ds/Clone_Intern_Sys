using FluentValidation;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Commands;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Commands
{
    public class AddUserToRoleCommandValidation : AbstractValidator<AddUserToRoleCommand>
    {
        public AddUserToRoleCommandValidation()
        {
            RuleFor(model => model.UserId)
                .NotEmpty().WithMessage("Chưa chọn User để thêm Role!");
            RuleFor(model => model.UserId)
                .NotEmpty().WithMessage("Tên Role không được để trống!");
        }
    }

    public class AddUserToRoleCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public AddUserToRoleCommand(string userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}

