using FluentValidation;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands
{

    public class VerifyCodeCommandValidator : AbstractValidator<CheckValidCode>
    {
        public VerifyCodeCommandValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("Email không được để trống!")
                .EmailAddress().WithMessage("Sai định dạng Email.");
            RuleFor(model => model.Code)
              .NotEmpty().WithMessage("Code không được để trống!");
        }
    }

    public class VerifyCodeCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Code { get; set; }

        public VerifyCodeCommand(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}
