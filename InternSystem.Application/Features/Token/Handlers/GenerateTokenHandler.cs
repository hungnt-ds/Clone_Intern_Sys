using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.Token.Commands;
using InternSystem.Application.Features.Token.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;


namespace InternSystem.Application.Features.Token.Handlers
{
    // Generate JWT  - backup by 'tqthai'
    public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, TokenResponse>
    {
        private readonly IMediator _mediator;
        private const string JwtKey = "6f1f3d28a486ec27d52cd5552954a81940d2a4c1d98";
        private readonly ITimeService _timeService;

        public GenerateTokenCommandHandler(IMediator mediator, ITimeService timeService)
        {
            _mediator = mediator;
            _timeService = timeService;
        }

        public async Task<TokenResponse> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            // Generate access token
            var accessToken = GenerateToken(request.UserId, request.Role, false);

            // Generate refresh token
            var refreshToken = GenerateToken(request.UserId, request.Role, true);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateToken(string userId, string role, bool isRefreshToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (isRefreshToken)
            {
                claims.Add(new Claim("isRefreshToken", "true"));
            }

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5284",
                audience: "InternSystem",
                claims: claims,
                expires: isRefreshToken ? _timeService.SystemTimeNow.AddHours(24).DateTime : _timeService.SystemTimeNow.AddMinutes(60).DateTime, // Token expiry time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
