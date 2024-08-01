using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CauhoiManagement.Queries;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.QuestionManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CauHoiCongNgheController : ControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public CauHoiCongNgheController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy câu hỏi công nghệ theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetCauHoiCongNgheById([FromQuery] GetCauHoiCongNgheByIdQuery query)
        {
            GetCauHoiCongNgheByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả câu hỏi công nghệ.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCauHoiCongNghe()
        {
            var query = new GetAllCauHoiCongNgheQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Tạo mới câu hỏi công nghệ.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCauHoiCongNghe([FromBody] CreateCauHoiCongNgheCommand command)
        {
            CreateCauHoiCongNgheResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật câu hỏi công nghệ.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCauHoiCongNghe([FromBody] UpdateCauHoiCongNgheCommand command)
        {
            UpdateCauHoiCongNgheResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa câu hỏi công nghệ.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCauHoiCongNghe([FromBody] DeleteCauHoiCongNgheCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
        /// <summary>
        /// Lấy danh sách câu hỏi công nghệ phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedCauHoiCongNghe([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedCauHoiCongNgheQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
