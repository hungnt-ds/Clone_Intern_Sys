using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Handlers
{
    public class UpdateEmailUserStatusCommandHandler : IRequestHandler<UpdateEmailUserStatusCommand, GetDetailEmailUserStatusResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateEmailUserStatusCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<GetDetailEmailUserStatusResponse> Handle(UpdateEmailUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existStatus = await _unitOfWork.EmailUserStatusRepository.GetByIdAsync(request.Id);

                if (existStatus == null || existStatus.IsDelete || !existStatus.IsActive)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy trạng thái hoặc đang không được kích hoạt");

                _mapper.Map(request, existStatus);
                string currentUserId = _userContextService.GetCurrentUserId();
                existStatus.LastUpdatedTime = _timeService.SystemTimeNow;
                existStatus.LastUpdatedBy = currentUserId;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<GetDetailEmailUserStatusResponse>(existStatus);
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
