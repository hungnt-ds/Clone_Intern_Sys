using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Handlers
{
    public class UpdateThongBaoHandler : IRequestHandler<UpdateThongBaoCommand, UpdateThongBaoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateThongBaoHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateThongBaoResponse> Handle(UpdateThongBaoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ThongBao? existingThongBao = await _unitOfWork.ThongBaoRepository.GetByIdAsync(request.Id) ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông báo");
                if(request.IdNguoiNhan == request.IdNguoiGui)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người nhận và người gửi không được giống nhau.");
                }
                existingThongBao = _mapper.Map(request, existingThongBao);
                existingThongBao.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingThongBao.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateThongBaoResponse>(existingThongBao);
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
