using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Handlers
{
    public class UpdateKyThucTapHandler : IRequestHandler<UpdateKyThucTapCommand, UpdateKyThucTapResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public UpdateKyThucTapHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateKyThucTapResponse> Handle(UpdateKyThucTapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                KyThucTap? existingKyThucTap = await _unitOfWork.KyThucTapRepository.GetByIdAsync(request.Id);
                if (existingKyThucTap == null || existingKyThucTap.IsDelete || !existingKyThucTap.IsActive)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Kỳ Thực Tập hoặc đang inactive");

                TruongHoc? existingTruong = await _unitOfWork.TruongHocRepository.GetByIdAsync(request.IdTruong);
                if (existingTruong == null || existingTruong.IsDelete || !existingTruong.IsActive)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Trường Học");

                // Kiểm tra số tuần thực tập và Ngày bắt đầu - Ngày kết thúc có phù hợp
                int count = existingTruong.SoTuanThucTap;
                int ngayThucTap = count * 7;
                int threshold = 2;
                var checkResult = ValidateInternshipDates(request, existingKyThucTap, ngayThucTap, threshold);
                if (!string.IsNullOrEmpty(checkResult))
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, checkResult);
                }

                existingKyThucTap.LastUpdatedBy = _userContextService.GetCurrentUserId();

                _mapper.Map(request, existingKyThucTap);
                existingKyThucTap.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateKyThucTapResponse>(existingKyThucTap);
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

        private string? ValidateInternshipDates(UpdateKyThucTapCommand request, KyThucTap existingKyThucTap, int ngayThucTap, int threshold)
        {
            if (request.NgayBatDau.HasValue && request.NgayKetThuc.HasValue)
            {
                TimeSpan dateRequest = (TimeSpan)(request.NgayKetThuc - request.NgayBatDau);
                int ngayRequest = dateRequest.Days;

                if (Math.Abs(ngayThucTap - ngayRequest) > threshold)
                {
                    return "Thời gian thực tập không nằm trong ngưỡng cho phép";
                }
            }
            else if (request.NgayBatDau.HasValue && !request.NgayKetThuc.HasValue)
            {
                TimeSpan dateRequest = (TimeSpan)(existingKyThucTap.NgayKetThuc - request.NgayBatDau);
                int ngayRequest = dateRequest.Days;

                if (Math.Abs(ngayThucTap - ngayRequest) > threshold)
                {
                    return "Thời gian thực tập không nằm trong ngưỡng cho phép";
                }
            }

            else if (!request.NgayBatDau.HasValue && request.NgayKetThuc.HasValue)
            {
                TimeSpan dateRequest = (TimeSpan)(request.NgayKetThuc - existingKyThucTap.NgayBatDau);
                int ngayRequest = dateRequest.Days;

                if (Math.Abs(ngayThucTap - ngayRequest) > threshold)
                {
                    return "Thời gian thực tập không nằm trong ngưỡng cho phép";
                }
            }

            if (request.NgayBatDau.HasValue && request.NgayKetThuc.HasValue)
            {
                if (request.NgayKetThuc < request.NgayBatDau)
                {
                    return "NgayKetThuc phải lớn hơn hoặc bằng NgayBatDau";
                }
            }

            return null;
        }
    }
}
