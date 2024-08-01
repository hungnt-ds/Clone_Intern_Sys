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
    public class UpdateLichPhongVanHandler : IRequestHandler<UpdateLichPhongVanCommand, UpdateLichPhongVanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;



        public UpdateLichPhongVanHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateLichPhongVanResponse> Handle(UpdateLichPhongVanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LichPhongVan? existingLichPhongVan = await _unitOfWork.LichPhongVanRepository.GetByIdAsync(request.Id);
                if (existingLichPhongVan == null || existingLichPhongVan.IsDelete == true)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");
                }

                InternInfo nguoiDuocPhongVan = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.IdNguoiDuocPhongVan) ??
                     throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Người được phỏng vấn");

                AspNetUser nguoiphongvan = await _unitOfWork.UserRepository.GetByIdAsync(request.IdNguoiPhongVan);
                if (nguoiphongvan == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người phỏng vấn không tồn tại.");


                existingLichPhongVan.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingLichPhongVan.LastUpdatedTime = _timeService.SystemTimeNow;
                _mapper.Map(request, existingLichPhongVan);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateLichPhongVanResponse>(existingLichPhongVan);
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
