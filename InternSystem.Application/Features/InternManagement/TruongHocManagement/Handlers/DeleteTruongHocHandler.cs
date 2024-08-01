using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Handlers
{
    public class DeleteTruongHocHandler : IRequestHandler<DeleteTruongHocCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteTruongHocHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteTruongHocCommand request, CancellationToken cancellationToken)
        {
            try
            {
                TruongHoc? searchResult = await _unitOfWork.TruongHocRepository.GetByIdAsync(request.Id);

                if (searchResult == null || searchResult.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy trường học");

                // Kiểm tra trước khi xóa id có ở bảng khác không
                var existKyThucTap = await _unitOfWork.KyThucTapRepository.GetKyThucTapByTruongHocId(request.Id);
                if (existKyThucTap.Any())
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì trường vẫn còn kỳ thực tập");
                }

                searchResult.DeletedBy = _userContextService.GetCurrentUserId();
                searchResult.DeletedTime = _timeService.SystemTimeNow;
                searchResult.IsActive = false;
                searchResult.IsDelete = true;

                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lưu.");
            }
        }
    }
}
