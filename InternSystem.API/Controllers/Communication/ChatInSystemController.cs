using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Models;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.Communication
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatInSystemController : ControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public ChatInSystemController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy lịch sử tin nhắn giữa người gửi và người nhận.
        /// </summary>
        /// <param name="idSender"></param>
        /// <param name="idReceiver"></param>
        /// <returns></returns>
        [HttpGet("get-message-history")]
        public async Task<ActionResult<List<GetMessageHistoryResponse>>> GetMessageHistory([FromQuery] string idSender, [FromQuery] string idReceiver)
        {
            var query = new GetMessageHistoryQuery
            {
                IdSender = idSender,
                IdReceiver = idReceiver
            };
            var messages = await _mediatorService.Send(query);
            return Ok(messages);
        }

        /// <summary>
        /// Gửi tin nhắn.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
        {
            var validator = new SendMessageCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var result = await _mediatorService.Send(command);
                if (result)
                {
                    return Ok("Message sent successfully");
                }
                else
                {
                    return StatusCode(500, "An error occurred while sending the message");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Xóa tin nhắn theo Id.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpDelete("delete-message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var command = new DeleteMessageCommand(messageId);
            var validator = new DeleteMessageCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var result = await _mediatorService.Send(command);
                if (result)
                {
                    return Ok("Message deleted successfully");
                }
                else
                {
                    return StatusCode(500, "An error occurred while deleting the message");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Cập nhật tin nhắn.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-message")]
        public async Task<IActionResult> UpdateMessage([FromBody] UpdateMessageCommand command)
        {
            var validator = new UpdateMessageCommandValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var result = await _mediatorService.Send(command);
                if (result)
                {
                    return Ok("Message updated successfully");
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the message");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
