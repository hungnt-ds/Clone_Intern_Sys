using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Commands
{
    public class DeleteUserRoleCommandValidation : AbstractValidator<DeleteUserRoleCommand>
    {
        public DeleteUserRoleCommandValidation()
        {
            RuleFor(model => model.UserId)
                .NotEmpty().WithMessage("Chưa chọn User để xóa Role!");
            RuleFor(model => model.RoleId)
                .NotEmpty().WithMessage("Role Id không được để trống!");
        }
    }

    public class DeleteUserRoleCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public DeleteUserRoleCommand(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
