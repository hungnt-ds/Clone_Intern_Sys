using FluentValidation;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("Email không được để trống!")
                .EmailAddress().WithMessage("Sai định dạng Email.");
        }
    }

    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }

        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
    }
}

