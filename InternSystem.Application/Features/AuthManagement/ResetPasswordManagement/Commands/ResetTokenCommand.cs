using FluentValidation;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands
{
    public class ResetTokenCommand : IRequest<ResetTokenResponse>
    {
        public ResetTokenRequest ResetTokenRequest { get; }
        public ResetTokenCommand(ResetTokenRequest resetTokenRequest)
        {
            ResetTokenRequest = resetTokenRequest;
        }
    }
}
