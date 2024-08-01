using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CauhoiManagement.Queries;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.QuestionManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CauHoiController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;
        public CauHoiController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }
        /// <summary>
        /// Lấy câu hỏi theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetCauHoiById([FromQuery] GetCauHoiByIdQuery query)
        {
            GetCauHoiByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả câu hỏi.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCauHoi()
        {
            var query = new GetAllCauHoiQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy câu hỏi theo nội dung.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-noi-dung")]
        public async Task<IActionResult> GetCauHoiByNoiDung([FromQuery] GetCauHoiByNoiDungQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Tạo mới câu hỏi.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCauHoi([FromBody] CreateCauHoiCommand command)
        {
            CreateCauHoiResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật câu hỏi.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCauHoi([FromBody] UpdateCauHoiCommand command)
        {
            UpdateCauHoiResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa câu hỏi.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCauHoi([FromBody] DeleteCauHoiCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
        /// <summary>
        /// Lấy danh sách câu hỏi phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedCauHoi([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedCauHoisQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
