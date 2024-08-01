using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Handlers
{
    public class UpdateCongNgheHandler : IRequestHandler<UpdateCongNgheCommand, UpdateCongNgheResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateCongNgheResponse> Handle(UpdateCongNgheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                CongNghe? existingCongNghe = await _unitOfWork.CongNgheRepository.GetByIdAsync(request.Id);
                if (existingCongNghe == null || existingCongNghe.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ");
                }

                existingCongNghe.LastUpdatedBy = currentUserId;
                existingCongNghe.LastUpdatedBy = currentUserId;
                existingCongNghe.LastUpdatedTime = _timeService.SystemTimeNow;
                existingCongNghe = _mapper.Map(request, existingCongNghe);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateCongNgheResponse>(existingCongNghe);
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