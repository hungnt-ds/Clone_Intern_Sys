using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Handlers
{
    public class CreateLichPhongVanHandler : IRequestHandler<CreateLichPhongVanCommand, CreateLichPhongVanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateLichPhongVanHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateLichPhongVanResponse> Handle(CreateLichPhongVanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                InternInfo nguoiDuocPhongVan = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.IdNguoiDuocPhongVan) ??
                     throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Người được phỏng vấn");
                
                AspNetUser nguoiphongvan = await _unitOfWork.UserRepository.GetByIdAsync(request.IdNguoiPhongVan);
                if (nguoiphongvan == null) 
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người phỏng vấn không tồn tại.");

                if (await _unitOfWork.LichPhongVanRepository.IsNguoiPhongVanConflict(request.IdNguoiPhongVan, request.ThoiGianPhongVan) ||
                    await _unitOfWork.LichPhongVanRepository.IsNguoiDuocPhongVanConflict(request.IdNguoiDuocPhongVan, request.ThoiGianPhongVan))
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Thời gian phỏng vấn bị trùng với lịch trình khác.");
                }
                var newLichPhongVan = _mapper.Map<LichPhongVan>(request);
                newLichPhongVan.CreatedBy = _userContextService.GetCurrentUserId();
                newLichPhongVan.LastUpdatedBy = newLichPhongVan.CreatedBy;
                newLichPhongVan.CreatedTime = _timeService.SystemTimeNow;
                newLichPhongVan.LastUpdatedTime = _timeService.SystemTimeNow;
                newLichPhongVan.TrangThai = true; // Trạng thái hoạt động
                newLichPhongVan = await _unitOfWork.LichPhongVanRepository.AddAsync(newLichPhongVan);

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateLichPhongVanResponse>(newLichPhongVan);
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
