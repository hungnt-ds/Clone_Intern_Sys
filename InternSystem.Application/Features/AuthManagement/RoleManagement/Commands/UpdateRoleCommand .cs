using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Commands
{
    public class UpdateRoleCommandValidation : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidation()
        {
            RuleFor(model => model.RoleId)
                .NotEmpty().WithMessage("Chưa chọn Role để Update!");
            RuleFor(model => model.NewRoleName)
                .NotEmpty().WithMessage("Tên mới không được để trống!");
        }
    }
    public class UpdateRoleCommand : IRequest<bool>
    {
        public string RoleId { get; set; }
        public string NewRoleName { get; set; }

        public UpdateRoleCommand(string roleId, string newRoleName)
        {
            RoleId = roleId;
            NewRoleName = newRoleName;
        }
    }
}

