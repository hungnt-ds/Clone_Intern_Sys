using InternSystem.Application.Common.Bases;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Queries;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserManagement.Queries;
using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Commands;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using Microsoft.AspNetCore.Mvc;

namespace InternSystem.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMediatorService _mediatorService;

        public UserController(IMediatorService mediatorService, IWebHostEnvironment webHostEnvironment)
        {
            _mediatorService = mediatorService;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Lấy người dùng theo họ và tên.
        /// </summary>
        /// <param name="hoVaTen"></param>
        /// <returns></returns>
        [HttpGet("get-by-name")]
        public async Task<IActionResult> GetUsersByHoVaTen(string hoVaTen)
        {
            var query = new GetUsersByHoVaTenQuery(hoVaTen);
            var users = await _mediatorService.Send(query);
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        /// <summary>
        /// Lấy tất cả người dùng.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-user")]
        public async Task<ActionResult<BaseResponseModel<IEnumerable<GetAllUserResponse>>>> GetAllUser()
        {
            var response = await _mediatorService.Send(new GetAllUserQuery());
            return Ok(new BaseResponseModel<IEnumerable<GetAllUserResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Lấy danh sách người dùng phân trang.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-paged")]
        public async Task<IActionResult> GetPagedUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedUsersQuery(pageNumber, pageSize);
            var response = await _mediatorService.Send(query);
            return Ok(new BaseResponseModel<PaginatedList<GetPagedUsersResponse>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response));
        }

        /// <summary>
        /// Xóa vai trò người dùng.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("delete-user-role")]
        public async Task<ActionResult> DeleteUserRole([FromBody] DeleteUserRoleCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
               
        /// <summary>
        /// Kích hoạt hoặc hủy kích hoạt người dùng.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("activate-deactivate-user")]
        public async Task<IActionResult> Deactivate([FromBody] ActiveUserCommand command)
        {
            var result = await _mediatorService.Send(command);
            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        /// <summary>
        /// Tải lên hình ảnh người dùng.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload/{id}")]
        public async Task<IActionResult> UploadImage(string id, IFormFile file)
        {
            var command = new UpdateUserImageCommand { UserId = id, File = file };
            var response = await _mediatorService.Send(command);

            if (!response.IsSuccess)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: response.ImageUrl));
        }
        //[HttpPost("upload/{id}")]
        //public async Task<IActionResult> UploadImage(string id, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("Không có tệp nào được tải lên.");
        //    }

        //    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }

        //    var command = new UpdateUserImageCommand { UserId = id, ImageUrl = "/images/" + uniqueFileName };
        //    var response = await _mediatorService.Send(command);

        //    if (!response.IsSuccess)
        //    {
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            System.IO.File.Delete(filePath);
        //        }
        //        return NotFound("Không tìm thấy người dùng.");
        //    }
        //    return Ok(new { response.ImageUrl });
        //}        
    }
}
