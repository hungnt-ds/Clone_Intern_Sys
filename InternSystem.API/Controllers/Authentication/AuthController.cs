using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Commands;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Queries;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.Token.Commands;
using InternSystem.Application.Features.Token.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;
        public AuthController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin người dùng hiện tại.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-current-user")]
        [Authorize]
        public async Task<ActionResult<GetUserDetailResponse>> GetLoggedUser()
        {
            var response = await _mediatorService.Send(new GetCurrentUserQuery());
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Đăng nhập.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _mediatorService.Send(new LoginCommand(request));
            return Ok(new BaseResponseModel<LoginResponse>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateUserResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật người dùng bởi quản trị viên.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-user-by-admin")]
        public async Task<ActionResult<CreateUserResponse>> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateUserResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Làm mới token. (Use for test)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequest request)
        {
            var response = await _mediatorService.Send(new RefreshTokenCommand(request));
            return Ok(new BaseResponseModel<TokenResponse>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Quên mật khẩu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = await _mediatorService.Send(new ForgotPasswordCommand(request.Email));
            return Ok(new BaseResponseModel<Unit>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Kiểm tra mã code hợp lệ để lấy lại mật khẩu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("check-valid-code")]
        public async Task<IActionResult> CheckValidCode([FromBody] CheckValidCode request)
        {
            var isValid = await _mediatorService.Send(new VerifyCodeCommand(request.Email, request.Code));
            return Ok(new BaseResponseModel<bool>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                isValid));
        }

        /// <summary>
        /// Đặt lại mật khẩu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _mediatorService.Send(new ResetPasswordCommand(request.Email, request.NewPassword, request.ConfirmPassword));
            return Ok(new BaseResponseModel<IdentityResult>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                response));
        }
    }
}
