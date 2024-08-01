using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Commands;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitriController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public VitriController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin vị trí theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetViTriById([FromQuery] GetViTriByIdQuery query)
        {
            GetViTriByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Lấy thông tin vị trí theo tên.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-vi-tri-by-ten")]
        public async Task<IActionResult> GetViTriByTen([FromQuery] GetViTriByTenQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Tạo mới vị trí.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateViTri([FromBody] CreateViTriCommand command)
        {
            CreateViTriResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả vị trí.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllViTri()
        {
            var query = new GetAllViTriQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Lấy danh sách vị trí phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedViTri([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedViTrisQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Cập nhật thông tin vị trí.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateViTri([FromBody] UpdateViTriCommand command)
        {
            UpdateViTriResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Xóa vị trí.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteViTri([FromBody] DeleteViTriCommand command)
        {
            bool response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

    }
}
