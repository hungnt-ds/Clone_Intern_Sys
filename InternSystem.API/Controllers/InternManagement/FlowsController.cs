using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Queries;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowsController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public FlowsController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }
        /// <summary>
        /// Tạo nhóm Zalo theo Id phỏng vấn.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        //[HttpPost("userNhomZalo/create-by-id-phongvan")]
        //public async Task<IActionResult> CreateUserNhomZaloByIdPhongvan([FromBody] CreateUserToNhomZaloByIdCommand command)
        //{
        //    var response = await _mediatorService.Send(command);
        //    return Ok(new BaseResponseModel(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: response));
        //}

        /// <summary>
        /// Tạo task của nhóm Zalo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("nhomZaloTask/create")]
        public async Task<IActionResult> CreateNhomZaloTask([FromBody] CreateNhomZaloTaskCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Thăng cấp intern lên leader.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("promote-intern-to-leader")]
        public async Task<IActionResult> InternToLeader([FromBody] PromoteMemberToLeaderCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật task của nhóm Zalo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("nhomZaloTask/update")]
        public async Task<IActionResult> UpdateNhomZaloTask([FromBody] UpdateNhomZaloTaskCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả task nhóm Zalo. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("nhomZaloTask/get-all")]
        public async Task<IActionResult> GetAllNhomZaloTask([FromQuery] GetNhomZaloTaskByQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa task của nhóm Zalo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("nhomZaloTask/delete")]
        public async Task<IActionResult> DeleteUserTask([FromBody] DeleteNhomZaloTaskCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status204NoContent,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
    }
}
