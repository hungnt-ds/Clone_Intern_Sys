using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Handlers
{
    public class DeleteUserViTriHandler : IRequestHandler<DeleteViTriCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteUserViTriHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteViTriCommand request, CancellationToken cancellationToken)
        {
            try
            {

                IQueryable<ViTri> allViTri = _unitOfWork.ViTriRepository.Entities;
                IQueryable<ViTri> activeViTri = allViTri
                    .Where(p => !p.IsDelete)
                    .Where(p => p.Id == request.Id);
                var viTri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");

                ViTri? existingViTri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.Id);
                if (existingViTri == null || existingViTri.IsDelete == true)
                    return false;

                var tempUservitri = await _unitOfWork.UserViTriRepository.GetAllAsync();
                var checkDeleteList = tempUservitri.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteList.Any(m => m.IdViTri == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Không thể xóa vị trí vì vẫn còn intern trong kỳ thực tập này.");
                }
                var tempCongNghe = await _unitOfWork.CongNgheRepository.GetAllAsync();
                var checkDeleteListCongnghe = tempCongNghe.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteListCongnghe.Any(m => m.IdViTri == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Không thể xóa cong nghệ vì vẫn còn intern trong kỳ thực tập này.");
                }
                var currentUserId = _userContextService.GetCurrentUserId();
                existingViTri.DeletedBy = currentUserId;
                existingViTri.LastUpdatedBy = currentUserId;
                existingViTri.DeletedTime = _timeService.SystemTimeNow;
                existingViTri.IsActive = false;
                existingViTri.IsDelete = true;

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
