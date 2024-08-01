using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Commands
{
    public class DeleteRoleCommandValidation : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidation()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Chưa chọn Role để xóa!");
        }
    }
    public class DeleteRoleCommand : IRequest<bool>
    {
        public string? Name { get; set; }
    }
}
