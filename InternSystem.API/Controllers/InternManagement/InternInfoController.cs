using System.Globalization;
using CsvHelper;
using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Queries;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries;
using InternSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.InternManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternInfoController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public InternInfoController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy thông tin thực tập sinh theo Id.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetInternInfoById([FromQuery] GetInternInfoByIdQuery query)
        {
            GetInternInfoByIdResponse response = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<GetInternInfoByIdResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Tạo thông tin thực tập sinh.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateInternInfo([FromBody] CreateInternInfoCommand command)
        {
            CreateInternInfoResponse response = await _mediatorService.Send(command);

            return Ok(new BaseResponseModel<CreateInternInfoResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Thực tập sinh tự cập nhật thông tin.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("self-update")]
        public async Task<IActionResult> SelfUpdateInternInfo([FromBody] SelfUpdateInternInfoCommand command)
        {
            UpdateInternInfoResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateInternInfoResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Cập nhật thông tin thực tập sinh.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateInternInfo([FromBody] UpdateInternInfoCommand command)
        {
            UpdateInternInfoResponse response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<UpdateInternInfoResponse>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa thông tin thực tập sinh.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteInternInfo([FromQuery] DeleteInternInfoCommand command)
        {
            bool response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Tải xuống mẫu CSV.
        /// </summary>
        /// <returns></returns>
        [HttpGet("csv/download-template")]
        public IActionResult DownloadCsvTemplate()
        {
            var template = new List<TemplateData>
    {
        new TemplateData()
    };
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(template);
                streamWriter.Flush();
                memoryStream.Position = 0;
                return File(memoryStream.ToArray(), "text/csv", "UserTemplate.csv");
            }
        }

        /// <summary>
        /// Tải lên file CSV.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("csv/upload")]
        public async Task<IActionResult> UploadCsv([FromForm] ImportCsvCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));

        }

        /// <summary>
        /// Tải lên file XLSX.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("xlsx/upload")]
        public async Task<IActionResult> UploadXlsx([FromForm] ImportXlsxCommand command)
        {
            var response = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        ///// <summary>
        ///// Lấy danh sách trạng thái email của người dùng.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("get-all-user-email-status")]
        //public async Task<ActionResult<GetDetailEmailUserStatusResponse>> GetAllUserEmailStatus()
        //{
        //    var status = await _mediatorService.Send(new GetAllEmailUserStatusQuery());
        //    return Ok(new BaseResponseModel(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: status));
        //}

        ///// <summary>
        ///// Lấy trạng thái email người dùng theo Id.
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //[HttpGet("get-user-email-status-by-id")]
        //public async Task<IActionResult> GetEmailUserStatusById([FromQuery] GetEmailUserStatusByIdQuery query)
        //{
        //    var result = await _mediatorService.Send(query);
        //    return Ok(new BaseResponseModel<GetDetailEmailUserStatusResponse>(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: result));
        //}

        ///// <summary>
        ///// Tạo trạng thái email cho người dùng.
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[HttpPost("create-email-user-status")]
        //public async Task<ActionResult<GetDetailEmailUserStatusResponse>> CreateEmailUserStatus([FromBody] CreateEmailUserStatusCommand command)
        //{
        //    GetDetailEmailUserStatusResponse result = await _mediatorService.Send(command);
        //    return Ok(new BaseResponseModel<GetDetailEmailUserStatusResponse>(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: result));
        //}

        ///// <summary>
        ///// Cập nhật trạng thái email người dùng.
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[HttpPut("update-email-user-status")]
        //public async Task<ActionResult<GetDetailEmailUserStatusResponse>> UpdateEmailUserStatus([FromBody] UpdateEmailUserStatusCommand command)
        //{
        //    GetDetailEmailUserStatusResponse result = await _mediatorService.Send(command);
        //    return Ok(new BaseResponseModel<GetDetailEmailUserStatusResponse>(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: result));
        //}

        ///// <summary>
        ///// Xóa trạng thái email người dùng.
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[HttpDelete("delete-email-user-status")]
        //public async Task<IActionResult> DeleteEmailUserStatus([FromBody] DeleteEmailUserStatusCommand command)
        //{
        //    bool result = await _mediatorService.Send(command);
        //    return Ok(new BaseResponseModel(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: result));
        //}

        /// <summary>
        /// Lấy thông tin thực tập sinh theo tên trường học.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("by-truonghoc-name")]
        public async Task<IActionResult> GetInternInfoByTruongHocName([FromQuery] GetInternInfoByTruongHocNameQuery query)
        {
            var internInfos = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<IEnumerable<GetInternInfoResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: internInfos));
        }

        /// <summary>
        /// Tìm kiếm thông tin thực tập sinh.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetInternInfoByTuKhoa([FromBody] GetInternInfoQuery query)
        {
            var internInfos = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
            statusCode: StatusCodes.Status200OK,
            code: ResponseCodeConstants.SUCCESS,
            data: internInfos));
        }

        /// <summary>
        /// Lọc thông tin thực tập sinh theo trạng thái.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-interinfo-by-status")]
        public async Task<IActionResult> GetFilteredInternInfosByStatus([FromQuery] GetFilteredInternInfosByStatusQuery query)
        {
            var result = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Lọc thông tin thực tập sinh theo trường học và ngày.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("filter-intern-by-truong-ngay")]
        public async Task<ActionResult<IEnumerable<InternInfo>>> GetFilteredInternInfo([FromQuery] GetFilteredInternInfoQuery query)
        {
            var result = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<IEnumerable<InternInfo>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        /// <summary>
        /// Lấy danh sách thông tin thực tập sinh phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedInternInfo([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedInternInfosQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel(
                 statusCode: StatusCodes.Status200OK,
                 code: ResponseCodeConstants.SUCCESS,
                 data: response));
        }
    }
}
