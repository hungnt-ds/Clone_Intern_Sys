using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Handlers
{
    public class DeleteCauHoiCongNgheHandler : IRequestHandler<DeleteCauHoiCongNgheCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteCauHoiCongNgheHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteCauHoiCongNgheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CauHoiCongNghe? cauHoiCongNghe = await _unitOfWork.CauHoiCongNgheRepository.GetByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Câu hỏi công nghệ không tồn tại hoặc đã bị xóa.");

                // Kiểm tra có PhongVan nào có là id của CauHoiCongNghe đang bị xóa không
                var phongVan = await _unitOfWork.PhongVanRepository.GetAllAsync();
                var listPhongVan = phongVan.Where(x => x.IsActive && !x.IsDelete).ToList();
                if (listPhongVan.Any(m => m.IdCauHoiCongNghe == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn phỏng vấn liên quan đến câu hỏi công nghệ này.");
                }

                string deletedBy = _userContextService.GetCurrentUserId();
                    cauHoiCongNghe.DeletedBy = deletedBy;
                    cauHoiCongNghe.DeletedTime = _timeService.SystemTimeNow;
                cauHoiCongNghe.IsActive = false;
                cauHoiCongNghe.IsDelete = true;

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
