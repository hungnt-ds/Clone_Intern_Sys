using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class KyThucTapController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public KyThucTapController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin thực tập theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetKyThucTapById([FromQuery] GetKyThucTapByIdQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<GetKyThucTapByIdResponse>
            (
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Lấy thông tin thực tập theo tên.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-name")]
        public async Task<IActionResult> GetKyThucTapByName([FromQuery] GetKyThucTapByNameQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetKyThucTapByNameResponse>>
            (
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Tạo mới thông tin thực tập.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateKiThucTap([FromBody] CreateKyThucTapCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateKyThucTapResponse>
            (
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Lấy danh sách tất cả thông tin thực tập.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllKyThucTap()
        {
            var query = new GetAllKyThucTapQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetAllKyThucTapResponse>>
            (
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Lấy danh sách kỳ thực tập phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedKyThucTap([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedKyThucTapsQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Cập nhật thông tin thực tập.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateKyThucTap([FromBody] UpdateKyThucTapCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateKyThucTapResponse>
            (
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Xóa thông tin thực tập.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteKyThucTap([FromBody] DeleteKyThucTapCommand command)
        {
            bool response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}

