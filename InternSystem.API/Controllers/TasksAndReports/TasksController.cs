using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Queries;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.TasksAndReports
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public TasksController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }
        /// <summary>
        /// Tạo công việc mới.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật thông tin công việc.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa công việc.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status204NoContent,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy thông tin công việc theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetTaskById([FromQuery] GetTaskByIdQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả công việc.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTask([FromQuery] GetAllTaskQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách task phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedTask([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedTasksQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }

        /// <summary>
        /// Lấy công việc theo mô tả.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-mo-ta")]
        public async Task<IActionResult> GetTaskByMoTa([FromQuery] GetTaskByMoTaQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy công việc theo nội dung.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-noi-dung")]
        public async Task<IActionResult> GetTaskByNoiDung([FromQuery] GetTaskByNoiDungQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }
    }
}
