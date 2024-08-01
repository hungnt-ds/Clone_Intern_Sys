using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.TasksAndReports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTasksController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public ReportTasksController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Tạo báo cáo công việc.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTaskReportCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật báo cáo công việc.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTaskReportCommand command)
        {
            TaskReportResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa báo cáo công việc.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteTaskReportCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status204NoContent,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy báo cáo công việc theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] GetTaskReportByIdQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách tất cả báo cáo công việc.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] GetTaskReportQuery query)
        {
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách report task phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedReportTask([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedReportTasksQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
