using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Handlers
{
    public class DeleteKyThucTapHandler : IRequestHandler<DeleteKyThucTapCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteKyThucTapHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteKyThucTapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                KyThucTap? existingKiThucTap = await _unitOfWork.KyThucTapRepository.GetByIdAsync(request.Id);
                if (existingKiThucTap == null || existingKiThucTap.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy kỳ thực tập.");

                // Kiểm tra trước khi xóa id có ở bảng khác không
                var existIntern = await _unitOfWork.InternInfoRepository.GetInternInfoByKyThucTapId(request.Id);
                if (existIntern.Any())
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn intern trong kỳ thực tập này");
                }

                existingKiThucTap.IsActive = false;
                existingKiThucTap.IsDelete = true;
                existingKiThucTap.DeletedBy = _userContextService.GetCurrentUserId();
                existingKiThucTap.DeletedTime = _timeService.SystemTimeNow;

                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra.");
            }
        }
    }
}
