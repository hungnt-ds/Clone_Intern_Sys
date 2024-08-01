using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Handlers
{
    public class DeleteNhomZaloCommandHandler : IRequestHandler<DeleteNhomZaloCommand, DeleteNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteNhomZaloCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<DeleteNhomZaloResponse> Handle(DeleteNhomZaloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUserId = _userContextService.GetCurrentUserId();

                var nhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.Id);
                if (nhomZalo == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo");
                }

                // Kiểm tra có UserNhomZalo nào có là id của NhomZalo đang bị xóa không
                //var userNhomZalos = await _unitOfWork.UserNhomZaloRepository.FindByNhomZaloIdAsync(nhomZalo.Id);
                var userNhomZalo = await _unitOfWork.UserNhomZaloRepository.GetAllAsync();
                var listUserNhomZalo = userNhomZalo.Where(u => u.IsActive && !u.IsDelete).ToList();
                if (listUserNhomZalo.Any(m => m.IdNhomZaloRieng == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn user liên quan đến nhóm zalo riêng này.");
                }
                if (listUserNhomZalo.Any(m => m.IdNhomZaloChung == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn user liên quan đến nhóm zalo chung này.");
                }

                // Kiểm tra có NhomZaloTask nào có là id của NhomZalo đang bị xóa không
                var nhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.GetAllAsync();
                var listNhomZaloTask = nhomZaloTask.Where(x => x.IsActive && !x.IsDelete).ToList();
                if (listNhomZaloTask.Any(m => m.NhomZaloId == request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Không thể xóa vì vẫn còn task liên quan đến group zalo này.");
                }

                nhomZalo.IsActive = false;
                nhomZalo.IsDelete = true;
                nhomZalo.DeletedBy = currentUserId;
                nhomZalo.DeletedTime = _timeService.SystemTimeNow;

                _unitOfWork.NhomZaloRepository.UpdateNhomZaloAsync(nhomZalo);
                await _unitOfWork.SaveChangeAsync();

                return new DeleteNhomZaloResponse { IsSuccessful = true };
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi");
            }
        }
    }
}
