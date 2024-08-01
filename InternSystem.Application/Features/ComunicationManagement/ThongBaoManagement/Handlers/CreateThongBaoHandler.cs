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
    public class CreateThongBaoHandler : IRequestHandler<CreateThongBaoCommand, CreateThongBaoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateThongBaoHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateThongBaoResponse> Handle(CreateThongBaoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                AspNetUser? existingNguoiNhan = await _unitOfWork.UserRepository.GetByIdAsync(request.IdNguoiNhan)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người nhận");

                AspNetUser? existingNguoiGui = await _unitOfWork.UserRepository.GetByIdAsync(request.IdNguoiGui)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người gửi");
                if (request.IdNguoiNhan == request.IdNguoiGui)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người nhận và người gửi không được giống nhau.");
                }
                var currentUser = _userContextService.GetCurrentUserId();

                ThongBao newThongBao = _mapper.Map<ThongBao>(request);
                newThongBao.LastUpdatedBy = currentUser;
                newThongBao.CreatedBy = currentUser;
                newThongBao.CreatedTime = _timeService.SystemTimeNow;
                newThongBao.LastUpdatedTime = _timeService.SystemTimeNow;

                newThongBao = await _unitOfWork.ThongBaoRepository.AddAsync(newThongBao);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateThongBaoResponse>(newThongBao);
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
