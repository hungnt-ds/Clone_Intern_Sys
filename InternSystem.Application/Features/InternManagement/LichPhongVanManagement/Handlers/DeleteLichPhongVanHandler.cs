using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Handlers
{
    public class DeleteLichPhongVanHandler : IRequestHandler<DeleteLichPhongVanCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteLichPhongVanHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteLichPhongVanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LichPhongVan? existingLPV = await _unitOfWork.LichPhongVanRepository.GetByIdAsync(request.Id)
                   ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");

                if (existingLPV.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");
                }
                var tempPhongvan = await _unitOfWork.PhongVanRepository.GetAllAsync();
                var checkDeleteList = tempPhongvan.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (tempPhongvan.Any(m => m.IdLichPhongVan == existingLPV.Id))
                {
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Không thể xóa vì vẫn còn intern trong kỳ thực tập này.");
                }
                existingLPV.DeletedBy = _userContextService.GetCurrentUserId();
                existingLPV.DeletedTime = _timeService.SystemTimeNow;
                existingLPV.IsActive = false;
                existingLPV.IsDelete = true;

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
