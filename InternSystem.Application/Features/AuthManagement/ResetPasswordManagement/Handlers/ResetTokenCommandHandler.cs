using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Handlers
{
    public class ResetTokenCommandHandler : IRequestHandler<ResetTokenCommand, ResetTokenResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITimeService _timeService;
        public ResetTokenCommandHandler(IConfiguration configuration, IUnitOfWork unitOfWork, ITimeService timeService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _timeService = timeService;
        }

        public async Task<ResetTokenResponse> Handle(ResetTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByRefreshTokenAsync(request.ResetTokenRequest.ResetToken);
                if (user == null || user.ResetTokenExpires <= _timeService.SystemTimeNow)
                {
                    throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Invalid or expired refresh token");
                }
                var newVerificationToken = GenerateVerificationToken(user.Id);
                var newResetToken = GenerateResetToken();

                user.ResetToken = newVerificationToken;
                user.ResetTokenExpires = _timeService.SystemTimeNow.AddHours(1);
                await _unitOfWork.UserRepository.UpdateUserAsync(user);

                return new ResetTokenResponse
                {
                    VerificationToken = newVerificationToken,
                    ResetToken = newResetToken
                };
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lưu.");
            }
        }

        private string GenerateVerificationToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiresDateTime = _timeService.SystemTimeNow.AddMinutes(30).DateTime;

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: new[] { new Claim(JwtRegisteredClaimNames.Sub, userId) },
                expires: expiresDateTime,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateResetToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
