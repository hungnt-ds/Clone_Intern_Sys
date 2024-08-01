using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.DashboardAndStatistics.GeneralManagement.Models;
using InternSystem.Application.Features.DashboardAndStatistics.GeneralManagement.Queries;
using InternSystem.Application.Features.DashboardAndStatistics.InternManagement.Queries;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.DashboardAndStatistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ApiControllerBase
    {
        private readonly IMediatorService _mediatorService;
        public DashboardController(IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        /// <summary>
        /// Lấy dữ liệu bảng điều khiển.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetDashboard()
        //{
        //    var query = new GetAllDashboardQuery();
        //    var response = await _mediatorService.Send(query);
        //    return Ok(new BaseResponseModel<GetAllDashboardResponse>(
        //            statusCode: StatusCodes.Status200OK,
        //            code: ResponseCodeConstants.SUCCESS,
        //            data: response));
        //}

        /// <summary>
        /// Lấy tổng số thực tập sinh trong khoảng thời gian từ ngày đến ngày.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("total-interns-by-day-to-day")]
        public async Task<IActionResult> GetTotalInternStudents(DateTime startDate, DateTime endDate)
        {
            var query = new GetTotalInternStudentsQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var totalInternStudents = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: totalInternStudents));
        }

        /// <summary>
        /// Lấy thống kê thực tập sinh theo trường học.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-intern-stats-by-school")]
        public async Task<IActionResult> GetInternStatsBySchool([FromQuery] GetInternStatsBySchoolIdQuery query)
        {
            var result = await _mediatorService.Send(query);

            return Ok(new BaseResponseModel<IEnumerable<GetInternStatsBySchoolIdResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
    }
}
