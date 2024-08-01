using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Handlers
{
    public class DeleteCauHoiHandler : IRequestHandler<DeleteCauHoiCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteCauHoiHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteCauHoiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CauHoi? existingCauHoi = await _unitOfWork.CauHoiRepository.GetByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi");

                if (existingCauHoi.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Câu hỏi đã bị xóa trước đó.");

                bool hasRelatedRecords = await _unitOfWork.CauHoiRepository.HasRelatedRecordsAsync(request.Id);
                if (hasRelatedRecords)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.NotFound, "Không thể xóa câu hỏi vì có các bản ghi liên quan.");
                }

                existingCauHoi.DeletedBy = _userContextService.GetCurrentUserId();
                existingCauHoi.DeletedTime = _timeService.SystemTimeNow;
                existingCauHoi.IsActive = false;
                existingCauHoi.IsDelete = true;

                _unitOfWork.CauHoiRepository.Update(existingCauHoi);
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