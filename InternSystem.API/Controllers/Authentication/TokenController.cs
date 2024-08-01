using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.Token.Commands;
using InternSystem.Application.Features.Token.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;
        public static Dictionary<string, GenerateTokenCommand> UserStore = new Dictionary<string, GenerateTokenCommand>();

        public TokenController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Tạo mã token xác thực cho người dùng dựa trên UserId và vai trò.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("generate-token")]
        public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
        {
            var tokenResponse = await _mediatorService.Send(new GenerateTokenCommand
            {
                UserId = request.UserId,
                Role = request.Role
            });

            return Ok(tokenResponse);
        }
    }
}
