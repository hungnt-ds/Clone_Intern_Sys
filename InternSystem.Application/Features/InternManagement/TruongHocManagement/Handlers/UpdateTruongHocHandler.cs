using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Handlers
{
    public class UpdateTruongHocHandler : IRequestHandler<UpdateTruongHocCommand, UpdateTruongHocResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateTruongHocHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateTruongHocResponse> Handle(UpdateTruongHocCommand request, CancellationToken cancellationToken)
        {
            try
            {
                TruongHoc? existingTruongHoc = await _unitOfWork.TruongHocRepository.GetByIdAsync(request.Id);
                if (existingTruongHoc == null || existingTruongHoc.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy trường học");


                _mapper.Map(request, existingTruongHoc);
                existingTruongHoc.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingTruongHoc.LastUpdatedTime = _timeService.SystemTimeNow;

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<UpdateTruongHocResponse>(existingTruongHoc);
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
