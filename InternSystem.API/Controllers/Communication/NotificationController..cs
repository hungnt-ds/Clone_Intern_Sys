using InternSystem.Application.Common.Constants;
using InternSystem.Domain.BaseException;
using InternSystem.Infrastructure;
using InternSystem.Infrastructure.Persistences.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.API.Controllers.Communication
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly RabbitMQHelper _rabbitMQHelper;
        private readonly ApplicationDbContext _dbContext;

        public NotificationController(RabbitMQHelper rabbitMQHelper, ApplicationDbContext dbContext)
        {
            _rabbitMQHelper = rabbitMQHelper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Đưa thông báo vào hàng đợi để gửi đi.
        /// </summary>
        /// <param name="notificationTask"></param>
        /// <returns></returns>
        [HttpPost("enqueue")]
        public async Task<IActionResult> EnqueueNotification([FromBody] NotificationTask notificationTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nguoiNhanExists = await _dbContext.Users.AnyAsync(u => u.Id == notificationTask.IdNguoiNhan);
            var nguoiGuiExists = await _dbContext.Users.AnyAsync(u => u.Id == notificationTask.IdNguoiGui);

            if (!nguoiNhanExists)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người nhận");
            }

            if (!nguoiGuiExists)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người gửi");
            }

            _rabbitMQHelper.EnqueueNotification(notificationTask);
            return Ok(new { Message = "Thông báo được xếp hàng thành công" });
        }
    }
}
