using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.GroupAndTeam
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNhomZaloController : ControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public UserNhomZaloController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Thêm người dùng vào nhóm Zalo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddUserToNhomZalo([FromBody] AddUserToNhomZaloCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng trong nhóm Zalo.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetAllUserNhomZalos))]
        public async Task<IActionResult> GetAllUserNhomZalos()
        {
            var query = new GetAllUserNhomZaloQuery();
            var response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<IEnumerable<GetUserNhomZaloResponse>>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách người dùng trong nhóm zalo phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedUserNhomZalos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedUserNhomZaloQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông tin người dùng trong nhóm Zalo theo Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetUserNhomZaloById))]
        public async Task<IActionResult> GetUserNhomZaloById(int id)
        {
            var query = new GetUserNhomZaloByIdQuery(id);
            var response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<GetUserNhomZaloResponse>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật thông tin người dùng trong nhóm Zalo.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-user-nhom-zalo")]
        public async Task<IActionResult> UpdateUserNhomZalo([FromBody] UpdateUserNhomZaloCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateUserNhomZaloResponse>
                (statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa người dùng khỏi nhóm Zalo theo Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-user-nhom-zalo")]
        public async Task<IActionResult> DeleteUserNhomZalo(int id)
        {
            var command = new DeleteUserNhomZaloCommand { Id = id };
            var response = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
    }
}
