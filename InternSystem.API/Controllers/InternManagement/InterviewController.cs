using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CommentManagement.Commands;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using InternSystem.Application.Features.InternManagement.CommentManagement.Queries;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Queries;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Commands;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Models;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Queries;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Queries;
using InternSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public InterviewController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Hiển thị danh sách email với chỉ số.
        /// </summary>
        [HttpGet("show-emails-with-indices")]
        public async Task<IActionResult> ShowEmailsWithIndices()
        {
            var emailsWithIndices = await _mediatorService.Send(new GetEmailsWithIndicesQuery());

            if (emailsWithIndices == null || !emailsWithIndices.Any())
            {
                return NotFound(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status404NotFound,
                    code: ResponseCodeConstants.ERROR,
                    data: "No emails with indices found"
                ));
            }

            return Ok(new BaseResponseModel<IEnumerable<EmailWithIndexResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: emailsWithIndices
            ));
        }

        /// <summary>
        /// Lấy danh sách loại email.
        /// </summary>
        /// <returns></returns>
        [HttpGet("show-email-types")]
        public ActionResult<IEnumerable<string>> GetEmailTypes()
        {
            var emailTypes = new List<string> { "Interview Date", "Interview Result", "Internship Time", "Internship Information" };
            return Ok(emailTypes);
        }

        /// <summary>
        /// Gửi các email đã chọn.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("send-emails")]
        public async Task<IActionResult> SendEmails([FromBody] SendEmailsRequest request)
        {
            var selectedEmails = await _mediatorService.Send(new SelectEmailsCommand(request.Indices));

            if (!selectedEmails.Any())
            {
                return BadRequest(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status400BadRequest,
                    code: ResponseCodeConstants.ERROR,
                    data: "No emails selected. Please select at least one email."
                ));
            }

            var result = await _mediatorService.Send(new SendEmailsCommand(selectedEmails, request.Subject, request.Body, request.EmailType));

            if (result)
            {
                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: ResponseCodeConstants.SUCCESS,
                    data: "Emails sent successfully"
                ));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status500InternalServerError,
                    code: ResponseCodeConstants.ERROR,
                    data: "Error sending email."
                ));
            }
        }

        /// <summary>
        /// Tạo lịch phỏng vấn mới.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create-lichphongvan")]
        public async Task<ActionResult<CreateLichPhongVanResponse>> CreateLichPhongVan(CreateLichPhongVanCommand command)
        {
            var result = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                message: "Tạo mới thành công.",
                data: result
            ));
        }

        /// <summary>
        /// Xem lịch phỏng vấn trong ngày hôm nay.
        /// </summary>
        /// <returns></returns>
        [HttpGet("view-lichphongvan-today")]
        public async Task<ActionResult> GetLichPhongVanByToday()
        {
            var result = await _mediatorService.Send(new GetLichPhongVanByTodayQuery());

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Lấy thông tin thành công.",
               data: result
           ));
        }

        /// <summary>
        /// Xem tất cả lịch phỏng vấn.
        /// </summary>
        /// <returns></returns>
        [HttpGet("view-all-lich-phong-van")]
        public async Task<ActionResult> GetAllLichPhongVan()
        {
            var result = await _mediatorService.Send(new GetAllLichPhongVanQuery());
            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Lấy thông tin thành công.",
               data: result
           ));
        }

        /// <summary>
        /// Xem lịch phỏng vấn theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("view-lich-phong-van-by-id")]
        public async Task<ActionResult> GetLichPhongVanById([FromQuery] GetLichPhongVanByIdQuery query)
        {
            var result = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Lấy thông tin thành công.",
               data: result
           ));
        }

        /// <summary>
        /// Xóa lịch phỏng vấn theo ID.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete-lich-phong-van-by-id")]
        public async Task<ActionResult> DeleteLichPhongVan([FromBody] DeleteLichPhongVanCommand command)
        {
            var result = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status204NoContent,
               code: ResponseCodeConstants.SUCCESS,
               message: "Xóa thành công.",
               data: result
           ));
        }

        /// <summary>
        /// Cập nhật lịch phỏng vấn theo ID.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-lich-phong-van-by-id")]
        public async Task<IActionResult> UpdateLichPhongVan([FromBody] UpdateLichPhongVanCommand command)
        {
            var result = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Cập nhật thành công.",
               data: result
           ));
        }

        /// <summary>
        /// Lấy thông tin thực tập sinh trong ngày phỏng vấn.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("get-inter-on-interview-day")]
        public async Task<IActionResult> GetInfoInterOfInterviewInDay([FromQuery] DateTime day)
        {
            var command = new GetFilteredInternInfoByDayQuery { Day = day };
            var result = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Lấy thông tin thành công.",
               data: result
           ));
        }

        /// <summary>
        /// Tạo phiếu phỏng vấn mới.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create-phong-van")]
        public async Task<ActionResult> CreatePhongVan(CreatePhongVanCommand command)
        {
            var response = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status201Created,
               code: ResponseCodeConstants.SUCCESS,
               message: "Tạo mới thành công.",
               data: response
           ));
        }

        /// <summary>
        /// Xem tất cả các phiếu phỏng vấn.
        /// </summary>
        /// <returns></returns>
        [HttpGet("view-all-phong-van")]
        public async Task<ActionResult> GetAllPhongVan()
        {
            var response = await _mediatorService.Send(new GetAllPhongVanQuery());
            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status201Created,
               code: ResponseCodeConstants.SUCCESS,
               message: "Lấy dữ liệu thành công.",
               data: response
           ));
        }

        /// <summary>
        /// Xem phiếu phỏng vấn theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("view-phong-van-by-id")]
        public async Task<ActionResult> GetPhongVanById([FromQuery] GetPhongVanByIdQuery query)
        {
            var response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Lấy dữ liệu thành công.",
               data: response
           ));
        }

        /// <summary>
        /// Cập nhật phiếu phỏng vấn theo ID.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-phong-van-by-id")]
        public async Task<IActionResult> UpdatePhongVan([FromBody] UpdatePhongVanCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               message: "Cập nhật thành công.",
               data: response
           ));
        }

        /// <summary>
        /// Xóa phiếu phỏng vấn theo ID.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete-phong-van-by-id")]
        public async Task<IActionResult> DeletePhongVan([FromBody] DeletePhongVanCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
               statusCode: StatusCodes.Status204NoContent,
               code: ResponseCodeConstants.SUCCESS,
               message: "Xóa thành công.",
               data: response
           ));
        }

        /// <summary>
        /// Lấy tất cả các bình luận.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-comments")]
        public async Task<IActionResult> GetAllComments()
        {
            var response = await _mediatorService.Send(new GetAllCommentsQuery());
            return Ok(new BaseResponseModel<IEnumerable<GetDetailCommentResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Lấy bình luận theo ID.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-comment-by-id")]
        public async Task<IActionResult> GetCommentById([FromQuery] GetCommentByIdQuery query)
        {
            GetDetailCommentResponse response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<GetDetailCommentResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }

        /// <summary>
        /// Tạo mới một bình luận.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create-comment")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand command)
        {
            GetDetailCommentResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<GetDetailCommentResponse>(
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật bình luận.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-comment")]
        public async Task<ActionResult<GetDetailCommentResponse>> UpdateComment([FromBody] UpdateCommentCommand command)
        {
            GetDetailCommentResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<GetDetailCommentResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa bình luận.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentCommand command)
        {
            bool response = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status204NoContent,
                code: ResponseCodeConstants.SUCCESS,
                data: response
            ));
        }
        /// <summary>
        /// Lấy danh sách phỏng vấn phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedInternView([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedPhongVanQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
