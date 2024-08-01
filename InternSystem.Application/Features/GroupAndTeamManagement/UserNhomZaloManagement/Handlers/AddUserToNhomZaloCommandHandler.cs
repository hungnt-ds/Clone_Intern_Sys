using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Handlers
{
    public class AddUserToNhomZaloCommandHandler : IRequestHandler<AddUserToNhomZaloCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public AddUserToNhomZaloCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(AddUserToNhomZaloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUserId = _userContextService.GetCurrentUserId();

                // Retrieve user and nhomZalo from repositories
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");

                var nhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.NhomZaloId);
                if (nhomZalo == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo");

                // Check if user is already in the group
                var existingUserNhomZalo = await _unitOfWork.UserNhomZaloRepository.GetByUserIdAndNhomZaloIdAsync(request.UserId, request.NhomZaloId);
                if (existingUserNhomZalo != null)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Người dùng đã thuộc nhóm Zalo");
                }
                // Create UserNhomZalo entity
                var userNhomZalo = new UserNhomZalo
                {
                    UserId = request.UserId,
                    IsMentor = request.IsMentor,
                    IsLeader = request.IsLeader,
                    IdNhomZaloChung = nhomZalo.IsNhomChung ? request.NhomZaloId : null,
                    IdNhomZaloRieng = nhomZalo.IsNhomChung ? null : request.NhomZaloId,
                    CreatedBy = currentUserId,
                    LastUpdatedBy = currentUserId,
                    CreatedTime = _timeService.SystemTimeNow,
                    LastUpdatedTime = _timeService.SystemTimeNow
                };

                // Add the entity to the repository
                _unitOfWork.UserNhomZaloRepository.AddAsync(userNhomZalo);

                // Save changes
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
