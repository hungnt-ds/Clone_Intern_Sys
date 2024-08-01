using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CongNgheManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.ProjectAndTechnologyManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongNgheController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public CongNgheController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Tạo mới công nghệ.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCongNghe([FromBody] CreateCongNgheCommand command)
        {
            CreateCongNgheResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateCongNgheResponse>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Cập nhật thông tin công nghệ.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCongNghe([FromBody] UpdateCongNgheCommand command)
        {
            UpdateCongNgheResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateCongNgheResponse>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                response
            ));
        }

        /// <summary>
        /// Xóa công nghệ.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCongNghe([FromBody] DeleteCongNgheCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                StatusCodes.Status204NoContent,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Lấy danh sách tất cả các công nghệ.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllCongNgheResponse>> GetAllCongNghe()
        {
            var query = new GetAllCongNgheQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetAllCongNgheResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông tin công nghệ theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetCongNgheById([FromQuery] GetCongNgheByIdQuery query)
        {
            GetCongNgheByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<GetCongNgheByIdResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông tin công nghệ theo tên.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-ten")]
        public async Task<IActionResult> GetCongNgheByTen([FromQuery] GetCongNghesByTenQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetCongNgheByTenResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
        /// <summary>
        /// Lấy danh sách công nghệ phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedCongNghe([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedCongNghesQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
