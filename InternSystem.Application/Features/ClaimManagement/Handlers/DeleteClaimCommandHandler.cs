using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ClaimManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ClaimManagement.Handlers
{
    public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteClaimCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<Unit> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.Id);
                if (claim == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy claim");
                }
                claim.DeletedBy = _userContextService.GetCurrentUserId();
                claim.DeletedTime = _timeService.SystemTimeNow;
                claim.IsActive = false;
                claim.IsDelete = true;
                await _unitOfWork.SaveChangeAsync();
                return Unit.Value;
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
