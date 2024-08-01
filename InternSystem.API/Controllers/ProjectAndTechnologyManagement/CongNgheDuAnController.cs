using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.ProjectAndTechnologyManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongNgheDuAnController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public CongNgheDuAnController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }


        /// <summary>
        /// Tạo mới công nghệ dự án.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCongNgheDuAn([FromBody] CreateCongNgheDuAnCommand command)
        {
            CreateCongNgheDuAnResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateCongNgheDuAnResponse>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Cập nhật thông tin công nghệ dự án.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCongNgheDuAn([FromBody] UpdateCongNgheDuAnCommand command)
        {
            UpdateCongNgheDuAnResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateCongNgheDuAnResponse>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                response
            ));
        }

        /// <summary>
        /// Xóa công nghệ dự án.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCongNgheDuAn([FromBody] DeleteCongNgheDuAnCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                StatusCodes.Status204NoContent,
                ResponseCodeConstants.SUCCESS,
                response));
        }

        /// <summary>
        /// Lấy danh sách tất cả các công nghệ dự án.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllCongNgheDuAnResponse>> GetAllCongNgheDuAn()
        {
            var query = new GetAllCongNgheDuAnQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetAllCongNgheDuAnResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông tin công nghệ dự án theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetCongNgheDuAnById([FromQuery] GetCongNgheDuAnByIdQuery query)
        {
            GetCongNgheDuAnByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<GetCongNgheDuAnByIdResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
        /// <summary>
        /// Lấy danh sách công nghệ dự án phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedCongNgheDuAn([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedCongNgheDuAnQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
