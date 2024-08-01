using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.ProjectAndTechnologyManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDuAnController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public UserDuAnController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin người dùng dự án theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserDuAnById([FromQuery] GetUserDuAnByIdQuery query)
        {
            GetUserDuAnByIdResponse response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<GetUserDuAnByIdResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Tạo mới người dùng dự án.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserDuAn([FromBody] CreateUserDuAnCommand command)
        {
            CreateUserDuAnResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<CreateUserDuAnResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng dự án.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUserDuAn()
        {
            var query = new GetAllUserDuAnQuery();
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetAllUserDuAnResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách người dùng và dự án phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged-users-du-an")]
        public async Task<IActionResult> GetPagedUserDuAn([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedUserDuAnQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Cập nhật thông tin người dùng dự án.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserDuAn([FromBody] UpdateUserDuAnCommand command)
        {
            UpdateUserDuAnResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateUserDuAnResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa người dùng dự án.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserDuAn([FromBody] DeleteUserDuAnCommand command)
        {
            bool response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
    }
}