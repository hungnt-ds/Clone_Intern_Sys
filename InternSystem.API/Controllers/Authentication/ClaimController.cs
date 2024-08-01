using InternSystem.API.Utilities;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ClaimManagement.Commands;
using InternSystem.Application.Features.ClaimManagement.Models;
using InternSystem.Application.Features.ClaimManagement.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IMediatorService _mediatorService;
        public ClaimController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy tất cả các claim.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<GetClaimResponse>> GetAllClaim()
        {
            var user = await _mediatorService.Send(new GetAllClaimQuery());
            return Ok(user);
        }

        /// <summary>
        /// Tạo mới một claim.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateClaim([FromBody] AddClaimCommand command)
        {
            var response = await _mediatorService.Send(command);
            if (!response.Errors.IsNullOrEmpty()) return StatusCode(500, response.Errors);
            return Ok(response);
        }

        /// <summary>
        /// Lấy thông tin claim theo ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetClaimResponse>> GetById(int id)
        {
            var claim = await _mediatorService.Send(new GetClaimByIdQuery(id));
            return Ok(claim);
        }

        /// <summary>
        /// Cập nhật thông tin claim.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateClaimCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new { message = result });
        }

        /// <summary>
        /// Xóa claim theo ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediatorService.Send(new DeleteClaimCommand { Id = id });
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}
