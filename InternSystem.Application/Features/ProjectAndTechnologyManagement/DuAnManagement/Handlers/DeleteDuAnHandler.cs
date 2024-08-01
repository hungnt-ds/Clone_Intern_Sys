using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Handlers
{
    public class DeleteDuAnHandler : IRequestHandler<DeleteDuAnCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public DeleteDuAnHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                DuAn? existingDA = await _unitOfWork.DuAnRepository.GetByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Dự Án không tồn tại");

                if (existingDA.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Dự Án");
                }

                // Kiểm tra trước khi xóa id có ở bảng khác không
                var tempUserDuan = await _unitOfWork.UserDuAnRepository.GetAllAsync();
                var checkDeleteList = tempUserDuan.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteList.Any(m => m.DuAnId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa dự án vì vẫn còn intern trong dự án này.");
                }

                // Kiểm tra trước khi xóa id có ở bảng khác không
                var tempTask = await _unitOfWork.TaskRepository.GetAllAsync();
                var checkDeleteTask = tempTask.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteTask.Any(m => m.DuAnId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa dự án vì vẫn còn task trong dự án này.");
                }

                // Kiểm tra trước khi xóa id có ở bảng khác không
                var tempCongNgheDuAn = await _unitOfWork.CongNgheDuAnRepository.GetAllAsync();
                var checkDeleteCongNgheDuAn = tempCongNgheDuAn.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteCongNgheDuAn.Any(m => m.IdDuAn == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa dự án vì vẫn còn công nghệ dự án trong dự án này.");
                }

                // Kiểm tra trước khi xóa id có ở bảng khác không
                var tempIntern = await _unitOfWork.InternInfoRepository.GetAllAsync();
                var checkDeleteIntern = tempIntern.Where(ch => ch.IsActive && !ch.IsDelete).ToList();
                if (checkDeleteIntern.Any(m => m.DuAnId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa dự án vì vẫn còn công nghệ dự án trong dự án này.");
                }

                var deleteBy = _userContextService.GetCurrentUserId();
                if (deleteBy.IsNullOrEmpty())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy Id của người dùng hiện tại");
                }


                existingDA.DeletedBy = deleteBy;
                existingDA.DeletedTime = _timeService.SystemTimeNow;
                existingDA.IsActive = false;
                existingDA.IsDelete = true;

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
