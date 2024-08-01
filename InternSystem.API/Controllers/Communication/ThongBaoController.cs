using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.Communication
{
    [Route("api/thongbao")]
    [ApiController]
    public class ThongBaoController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public ThongBaoController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Tạo thông báo mới.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateThongBao([FromBody] CreateThongBaoCommand command)
        {
            CreateThongBaoResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateThongBaoResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật thông báo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateThongBao([FromBody] UpdateThongBaoCommand command)
        {
            UpdateThongBaoResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateThongBaoResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa thông báo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteThongBao([FromBody] DeleteThongBaoCommand command)
        {
            DeleteThongBaoResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<DeleteThongBaoResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông báo theo Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("by-id")]
        public async Task<IActionResult> GetThongBaoById([FromQuery] int id)
        {
            GetThongBaoByIdResponse response = await _mediatorService.Send(new GetThongBaoByIdQuery()
            {
                Id = id
            });
            return Ok(new BaseResponseModel<GetThongBaoByIdResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả thông báo.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllThongBao()
        {
            var response = await _mediatorService.Send(new GetAllThongBaoQuery());
            return Ok(new BaseResponseModel<IEnumerable<GetAllThongBaoResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách thông báo phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedThongBao([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedThongBaosQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
