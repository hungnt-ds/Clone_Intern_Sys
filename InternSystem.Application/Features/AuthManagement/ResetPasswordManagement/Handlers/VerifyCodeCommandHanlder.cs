using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Handlers
{
    public class VerifyCodeCommandHanlder : IRequestHandler<VerifyCodeCommand, bool>
    {
        private readonly UserManager<AspNetUser> _userManager;

        public VerifyCodeCommandHanlder(UserManager<AspNetUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email) ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");

                var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.EmailCode, request.Code);
                return verificationResult != PasswordVerificationResult.Failed;
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
