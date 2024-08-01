using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Handlers
{
    public class DeleteUserViTriHandler : IRequestHandler<DeleteUserViTriCommand, bool>
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

        public async Task<bool> Handle(DeleteUserViTriCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                UserViTri? existingUserViTri = await _unitOfWork.UserViTriRepository.GetByIdAsync(request.Id);
                if (existingUserViTri == null || existingUserViTri.IsDelete == true)
                    return false;

                existingUserViTri.DeletedBy = currentUserId;
                existingUserViTri.DeletedBy = currentUserId;
                existingUserViTri.DeletedTime = _timeService.SystemTimeNow;
                existingUserViTri.IsActive = false;
                existingUserViTri.IsDelete = true;

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
