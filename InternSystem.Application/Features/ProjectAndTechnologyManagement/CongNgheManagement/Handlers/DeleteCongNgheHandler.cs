using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Handlers
{
    public class DeleteCongNgheHandler : IRequestHandler<DeleteCongNgheCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteCongNgheHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteCongNgheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CongNghe? existingCongNghe = await _unitOfWork.CongNgheRepository.GetByIdAsync(request.Id);
                if (existingCongNghe == null || existingCongNghe.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ");
                }

                // Kiểm tra trước khi xóa id có ở bảng khác không
                bool hasRelatedRecords = await _unitOfWork.CongNgheRepository.HasRelatedRecordsAsync(request.Id);
                if (hasRelatedRecords)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.NotFound, "Không thể xóa công nghệ vì có các bản ghi liên quan.");
                }

                existingCongNghe.DeletedBy = _userContextService.GetCurrentUserId();
                existingCongNghe.DeletedTime = _timeService.SystemTimeNow;
                existingCongNghe.IsActive = false;
                existingCongNghe.IsDelete = true;

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