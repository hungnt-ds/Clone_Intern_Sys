using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserViTriController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public UserViTriController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin vị trí của người dùng theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserViTriById([FromQuery] GetUserViTriByIdQuery query)
        {
            GetUserViTriByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Tạo mới vị trí của người dùng.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserViTri([FromBody] CreateUserViTriCommand command)
        {
            CreateUserViTriResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
             statusCode: StatusCodes.Status200OK,
             code: ResponseCodeConstants.SUCCESS,
             data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả vị trí của người dùng.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUserDuAn()
        {
            var query = new GetAllUserViTriQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Lấy danh sách người dùng trong vị trí phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedUserViTris([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedUserViTriQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Cập nhật vị trí của người dùng.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserViTri([FromBody] UpdateUserViTriCommand command)
        {
            UpdateUserViTriResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: response));
        }

        /// <summary>
        /// Xóa vị trí của người dùng.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserViTri([FromBody] DeleteUserViTriCommand command)
        {
            bool response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: response));
        }

    }
}
