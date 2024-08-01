using FluentValidation;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.LoginManagement.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginRequest>
    {
        public LoginCommandValidator()
        {
            RuleFor(model => model.Username)
                .NotEmpty().WithMessage("Username không được để trống!");
            RuleFor(model => model.Password)
              .NotEmpty().WithMessage("Mật khẩu không được để trống!");   
        }
    }
    public class LoginCommand : IRequest<LoginResponse>
    {
        public LoginRequest LoginRequest { get; set; }

        public LoginCommand(LoginRequest loginRequest)
        {
            LoginRequest = loginRequest;
        }
    }
    
}
