using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Commands;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Queries;
using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public RoleController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }
        /// <summary>
        /// Lấy tất cả các vai trò (roles).
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<GetRoleResponse>> GetAllRole()
        {
            var user = await _mediatorService.Send(new GetRoleQuery());
            return Ok(new BaseResponseModel<IEnumerable<GetRoleResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: user));
        }

        /// <summary>
        /// Lấy danh sách role phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedRole([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedRolesQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Tạo mới một vai trò (role).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AddRoleCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Cập nhật thông tin vai trò (role).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Xóa vai trò (role).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRoleCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Thêm người dùng vào vai trò (role).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Lấy danh sách vai trò của người dùng theo UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-user-role-by-userId/{userId}")]
        public async Task<ActionResult<IEnumerable<IdentityUserRole<string>>>> GetByUserId(string userId)
        {
            var query = new GetAspNetUserRoleByUserIdQuery { UserId = userId };
            var result = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Lấy danh sách người dùng thuộc vai trò theo RoleId.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("get-user-role-by-roleId/{roleId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetByRoleId(string roleId)
        {
            var query = new GetAspNetUserRoleByRoleIdQuery { RoleId = roleId };
            var result = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpGet("get-role-by-roleId/{roleId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoleByRoleId(string roleId)
        {
            var query = new GetRoleByIdQuery { RoleId = roleId };
            var result = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Lấy tất cả các cặp UserId và RoleId của người dùng và vai trò.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-user-role")]
        public async Task<ActionResult<IEnumerable<(string UserId, string RoleId)>>> GetAllUserRole()
        {
            var query = new GetAllAspNetUserRoleQuery();
            var result = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<UserRoleDto>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
    }
}
