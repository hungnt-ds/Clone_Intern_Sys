using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Handlers
{
    public class DeleteInternInfoHandler : IRequestHandler<DeleteInternInfoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public DeleteInternInfoHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteInternInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                InternInfo? intern = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.Id);
                if (intern == null || intern.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tồn tại thông tin của thực tập sinh");

                // Kiểm tra có EmailUserStatus nào có là id của InternInfo đang bị xóa không
                var emailUserStatus = await _unitOfWork.EmailUserStatusRepository.GetAllAsync();
                var listEmailUserStatus = emailUserStatus.Where(x => x.IsActive && !x.IsDelete).ToList();
                if (listEmailUserStatus.Any(m => m.IdNguoiNhan == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn email user status liên quan đến thông tin thực tập sinh này.");
                }

                // Kiểm tra có LichPhongVan nào có là id của InternInfo đang bị xóa không
                var lichPhongVan = await _unitOfWork.LichPhongVanRepository.GetAllAsync();
                var listLichPhongVan = lichPhongVan.Where(x => x.IsActive && !x.IsDelete).ToList();
                if (lichPhongVan.Any(m => m.IdNguoiDuocPhongVan == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn lịch phỏng vấn liên quan đến thông tin thực tập sinh này.");
                }

                // Kiểm tra có Comment nào có là id của InternInfo đang bị xóa không
                var comment = await _unitOfWork.CommentRepository.GetAllAsync();
                var listComment = comment.Where(x => x.IsActive && !x.IsDelete).ToList();
                if (listComment.Any(m => m.IdNguoiDuocComment == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn comment liên quan đến thông tin thực tập sinh này.");
                }

                intern.DeletedBy = currentUserId;
                intern.DeletedTime = _timeService.SystemTimeNow;
                intern.IsActive = false;
                intern.IsDelete = true;
                await _unitOfWork.SaveChangeAsync();

                return true;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi.");
            }
        }
    }
}
