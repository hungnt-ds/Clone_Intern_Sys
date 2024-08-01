using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Commands;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.AuthManagement.LoginManagement.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly int _exAccessToken;
        private readonly int _exRefreshToken;
        private readonly ITimeService _timeService;

        public LoginCommandHandler(IConfiguration configuration, UserManager<AspNetUser> userManager, ITimeService timeService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _exAccessToken = int.Parse(_configuration["Jwt:ExpirationAccessToken"]!);
            _exRefreshToken = int.Parse(_configuration["Jwt:ExpirationRefreshToken"]!);
            _timeService = timeService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.LoginRequest.Username) ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Tên đăng nhập hoặc mật khẩu không đúng");

                var checkPassword = await _userManager.CheckPasswordAsync(user, request.LoginRequest.Password);
                if (!checkPassword)
                {
                    throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthenticated, "Tên đăng nhập hoặc mật khẩu không đúng");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userRole = roles.FirstOrDefault();
                if (userRole == null)
                {
                    throw new ErrorException(StatusCodes.Status403Forbidden, ErrorCode.UnAuthorized, "Nguời dùng chưa được cấp quyền");
                }

                // Generate access token
                var accessToken = GenerateToken(user.Id, userRole, false);
                // Generate refresh token
                var refreshToken = GenerateToken(user.Id, userRole, true);

                // Save Database
                user.VerificationToken = accessToken;
                user.ResetToken = refreshToken;
                user.VerificationTokenExpires = _timeService.SystemTimeNow.AddHours(_exRefreshToken);
                user.VerificationTokenExpires = _timeService.SystemTimeNow.AddHours(_exRefreshToken);
                user.ResetTokenExpires = _timeService.SystemTimeNow.AddMinutes(_exAccessToken);
                await _userManager.UpdateAsync(user);
                return new LoginResponse
                {
                    VerificationToken = accessToken,
                    ResetToken = refreshToken,
                    UserId = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = userRole
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

        private string GenerateToken(string userId, string role, bool isRefreshToken)
        {
            try
            {
                var keyString = _configuration["Jwt:Key"]
                               ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "JWT key is not configured.");
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim("Id", userId),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                if (isRefreshToken)
                {
                    claims.Add(new Claim("isRefreshToken", "true"));
                }

                DateTime expiresDateTime;
                if (isRefreshToken)
                {
                    expiresDateTime = _timeService.SystemTimeNow.AddHours(_exRefreshToken).DateTime;
                }
                else
                {
                    expiresDateTime = _timeService.SystemTimeNow.AddMinutes(_exAccessToken).DateTime;
                }


                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expiresDateTime,
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
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
    }
}
