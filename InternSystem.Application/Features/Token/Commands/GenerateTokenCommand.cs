using InternSystem.Application.Features.Token.Models;
using MediatR;


namespace InternSystem.Application.Features.Token.Commands
{
    public class GenerateTokenCommand : IRequest<TokenResponse>
    {
        public string UserId { get; set; }
        public string Role { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }

    }
}
