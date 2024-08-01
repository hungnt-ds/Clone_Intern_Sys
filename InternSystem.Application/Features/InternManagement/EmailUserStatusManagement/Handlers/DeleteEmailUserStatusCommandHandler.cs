using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Handlers
{
    public class DeleteEmailUserStatusCommandHandler : IRequestHandler<DeleteEmailUserStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteEmailUserStatusCommandHandler(IUnitOfWork unitOfWork,
            IUserContextService userContextService,
            ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteEmailUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existStatus = await _unitOfWork.EmailUserStatusRepository.GetByIdAsync(request.Id);
                if (existStatus == null || existStatus.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Email");

                string currentUserId = _userContextService.GetCurrentUserId();

                existStatus.DeletedBy = currentUserId;
                existStatus.DeletedTime = _timeService.SystemTimeNow;
                existStatus.IsActive = false;
                existStatus.IsDelete = true;

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
