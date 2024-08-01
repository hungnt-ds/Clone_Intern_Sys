using FluentValidation;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Commands;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Commands
{
    public class ActiveUserCommandValidation : AbstractValidator<ActiveUserCommand>
    {
        public ActiveUserCommandValidation()
        {
            RuleFor(model => model.UserId)
                .NotEmpty().WithMessage("Chưa chọn User!");
            RuleFor(model => model.IsActive)
                .NotNull().WithMessage("Trạng thái kích hoạt không được để trống.")
                .Must(value => value == true || value == false).WithMessage("Trạng thái kích hoạt phải là true hoặc false.");
        }
    }

    public class ActiveUserCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
        public bool IsActive { get; set; }
        public ActiveUserCommand(string userId, bool isActive)
        {
            UserId = userId;
            IsActive = isActive;
        }
    }
}
