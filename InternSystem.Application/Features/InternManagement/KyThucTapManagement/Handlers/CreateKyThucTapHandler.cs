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
    public class CreateKyThucTapHandler : IRequestHandler<CreateKyThucTapCommand, CreateKyThucTapResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public CreateKyThucTapHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<CreateKyThucTapResponse> Handle(CreateKyThucTapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<KyThucTap> existingKyThucTap = await _unitOfWork.KyThucTapRepository.GetKyThucTapsByNameAsync(request.Ten);
                existingKyThucTap = existingKyThucTap.Where(e => e.IdTruong == request.IdTruong);
                if (existingKyThucTap.Any(ktt => ktt.Ten.ToLowerInvariant() == request.Ten.ToLowerInvariant()))
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "Kỳ thực tập đã tồn tại");

                TruongHoc? existingTruong = await _unitOfWork.TruongHocRepository.GetByIdAsync(request.IdTruong);
                if (existingTruong == null || existingTruong.IsDelete || !existingTruong.IsActive)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy trường học");

                // Kiểm tra ngày thực tập và số tuần thực tập của trường có khớp nhau không
                int count = existingTruong.SoTuanThucTap;
                int ngaythuctap = count * 7;
                TimeSpan dateRequest = request.NgayKetThuc - request.NgayBatDau;
                int ngayrequest = dateRequest.Days;
                int threshold = 2;
                if (Math.Abs(ngaythuctap - ngayrequest) < threshold)
                    throw new ErrorException(
                        StatusCodes.Status400BadRequest,
                        ResponseCodeConstants.BADREQUEST,
                        "Số ngày thực tập không phù hợp với số tuần thực tập của trường học."
                    );

                KyThucTap newKyThucTap = _mapper.Map<KyThucTap>(request);
                var currentUser = _userContextService.GetCurrentUserId();
                newKyThucTap.CreatedBy = currentUser;
                newKyThucTap.LastUpdatedBy = currentUser;
                newKyThucTap.LastUpdatedTime = newKyThucTap.CreatedTime;
                newKyThucTap.LastUpdatedBy = newKyThucTap.CreatedBy;

                newKyThucTap = await _unitOfWork.KyThucTapRepository.AddAsync(newKyThucTap);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<CreateKyThucTapResponse>(newKyThucTap);
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra.");
            }
        }
    }
}
