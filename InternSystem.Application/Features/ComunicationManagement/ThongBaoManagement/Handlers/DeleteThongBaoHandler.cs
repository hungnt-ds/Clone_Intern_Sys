using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Handlers
{
    public class DeleteThongBaoHandler : IRequestHandler<DeleteThongBaoCommand, DeleteThongBaoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteThongBaoHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<DeleteThongBaoResponse> Handle(DeleteThongBaoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingThongBao = await _unitOfWork.ThongBaoRepository.GetByIdAsync(request.Id) ??
                               throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông báo");

                existingThongBao.DeletedTime = _timeService.SystemTimeNow;
                existingThongBao.DeletedBy = _userContextService.GetCurrentUserId();
                existingThongBao.IsActive = false;
                existingThongBao.IsDelete = true;
                await _unitOfWork.SaveChangeAsync();

                return new DeleteThongBaoResponse(); 
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
