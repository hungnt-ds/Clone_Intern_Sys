using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.GroupAndTeam
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhomZaloController : ControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public NhomZaloController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Tạo mới nhóm Zalo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateNhomZalo([FromBody] CreateNhomZaloCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateNhomZaloResponse>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật thông tin nhóm Zalo.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNhomZalo(int id, [FromBody] UpdateNhomZaloCommand command)
        {
            var wrapper = new UpdateNhomZaloCommandWrapper
            {
                Id = id,
                Command = command
            };

            var response = await _mediatorService.Send(wrapper);

            return Ok(new BaseResponseModel<UpdateNhomZaloResponse>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa nhóm Zalo theo Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteNhomZalo(int id)
        {
            var command = new DeleteNhomZaloCommand { Id = id };
            var response = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả các nhóm Zalo.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllNhomZalos()
        {
            var query = new GetAllNhomZaloQuery();
            var response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<IEnumerable<GetNhomZaloResponse>>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông tin nhóm Zalo theo Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetNhomZaloById(int id)
        {
            var query = new GetNhomZaloByIdQuery(id);
            var response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<GetNhomZaloResponse>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy nhóm Zalo theo tên.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("get-by-name")]
        public async Task<IActionResult> GetNhomZaloByName(string name)
        {
            var query = new GetNhomZaloByNameQuery(name);
            var response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<GetNhomZaloResponse>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
    }
}
