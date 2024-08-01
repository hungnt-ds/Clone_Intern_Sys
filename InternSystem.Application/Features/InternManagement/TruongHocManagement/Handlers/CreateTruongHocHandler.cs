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
    public class CreateTruongHocHandler : IRequestHandler<CreateTruongHocCommand, CreateTruongHocResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public CreateTruongHocHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateTruongHocResponse> Handle(CreateTruongHocCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var existingTruong = await _unitOfWork.TruongHocRepository.GetTruongHocsByTenAsync(request.Ten);
                if (existingTruong.Any())
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "Trường học đã tồn tại");

                TruongHoc newTruongHoc = _mapper.Map<TruongHoc>(request);
                newTruongHoc.CreatedBy = _userContextService.GetCurrentUserId();
                newTruongHoc.LastUpdatedTime = _timeService.SystemTimeNow;
                newTruongHoc.LastUpdatedBy = _userContextService.GetCurrentUserId();
                newTruongHoc = await _unitOfWork.TruongHocRepository.AddAsync(newTruongHoc);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<CreateTruongHocResponse>(newTruongHoc);
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
