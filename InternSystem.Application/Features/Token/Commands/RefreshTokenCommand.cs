using InternSystem.Application.Features.Token.Models;
using MediatR;


namespace InternSystem.Application.Features.Token.Commands
{
    public class RefreshTokenCommand : IRequest<TokenResponse>
    {
        public RefreshTokenRequest RefreshToken { get; set; }
        public RefreshTokenCommand(RefreshTokenRequest loginRequest)
        {
            RefreshToken = loginRequest;
        }
    }
}
