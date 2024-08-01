using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Handlers
{
    public class UpdateInternInfoHandler : IRequestHandler<UpdateInternInfoCommand, UpdateInternInfoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateInternInfoHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateInternInfoResponse> Handle(UpdateInternInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                InternInfo existingIntern = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.Id);
                if (existingIntern == null || existingIntern.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thực tập sinh");

                TruongHoc? existingTruong = await _unitOfWork.TruongHocRepository.GetByIdAsync(request.IdTruong);
                if (existingTruong == null || existingTruong.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Trường Học");
                _mapper.Map(request, existingIntern);

                existingIntern.LastUpdatedTime = _timeService.SystemTimeNow;
                existingIntern.LastUpdatedBy = _userContextService.GetCurrentUserId();
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateInternInfoResponse>(existingIntern);
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
