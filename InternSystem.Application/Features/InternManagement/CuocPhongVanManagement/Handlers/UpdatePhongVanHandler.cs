using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Handlers
{
    public class UpdatePhongVanHandler : IRequestHandler<UpdatePhongVanCommand, UpdatePhongVanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdatePhongVanHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdatePhongVanResponse> Handle(UpdatePhongVanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                PhongVan? existingLichPhongVan = await _unitOfWork.PhongVanRepository.GetByIdAsync(request.Id);

                if (existingLichPhongVan == null || existingLichPhongVan.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");

                existingLichPhongVan.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingLichPhongVan.LastUpdatedTime = _timeService.SystemTimeNow;
                _mapper.Map(request, existingLichPhongVan);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdatePhongVanResponse>(existingLichPhongVan);
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
