using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IdentityResult>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITimeService _timeService;

        public ResetPasswordCommandHandler(UserManager<AspNetUser> userManager, IEmailService emailService, ITimeService timeService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _timeService = timeService;
        }

        public async Task<IdentityResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");
                }

                /*
                 * var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.EmailCode, request.Code);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Invalid token." });
                }
                */

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
                if (result.Succeeded)
                {
                    //user.EmailCode = null; 
                    await _userManager.UpdateAsync(user);
                    var selectedEmail = new List<string> { request.Email };
                    var time = _timeService.SystemTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
                    await _emailService.SendEmailAsync(selectedEmail, "Your password has changed", $"Your password has changed successfully at: {time}");
                    return IdentityResult.Success;
                }

                return result;
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
