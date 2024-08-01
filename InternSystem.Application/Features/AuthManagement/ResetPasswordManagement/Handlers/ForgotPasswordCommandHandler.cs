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
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(UserManager<AspNetUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email) ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                var code = new Random().Next(1000, 9999).ToString();
                var codeHash = _userManager.PasswordHasher.HashPassword(user, code);

                user.EmailCode = codeHash;
                await _userManager.UpdateAsync(user);

                var selectedEmail = new List<string> { request.Email };
                await _emailService.SendEmailAsync(selectedEmail, "Password Reset Code", $"Your reset code is: {code}");
                return Unit.Value;
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
