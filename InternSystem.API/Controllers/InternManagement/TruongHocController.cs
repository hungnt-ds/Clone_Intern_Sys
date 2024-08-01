using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruongHocController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public TruongHocController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin trường học theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetTruongHocById([FromQuery] GetTruongHocByIdQuery query)
        {
            GetTruongHocByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<GetTruongHocByIdResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách trường học theo tên.
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        [HttpGet("get-by-name")]
        public async Task<IActionResult> GetTruongHocsByTen(string ten)
        {
            try
            {
                var query = new GetTruongHocByTenQuery(ten);
                var truongHocs = await _mediatorService.Send(query);
                return Ok(new BaseResponseModel(
                    statusCode: StatusCodes.Status200OK,
                    code: ResponseCodeConstants.SUCCESS,
                    data: truongHocs));
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ex.ToString());
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả trường học.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTruongHoc()
        {
            var query = new GetAllTruongHocQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetAllTruongHocResponse>>(
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
        public async Task<IActionResult> GetPagedTruongHoc([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedTruongHocsQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Tạo mới thông tin trường học.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateTruongHoc([FromBody] CreateTruongHocCommand command)
        {
            CreateTruongHocResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateTruongHocResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật thông tin trường học.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTruongHoc([FromBody] UpdateTruongHocCommand command)
        {
            UpdateTruongHocResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateTruongHocResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa thông tin trường học.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTruongHoc([FromBody] DeleteTruongHocCommand command)
        {
            bool response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
    }
}
