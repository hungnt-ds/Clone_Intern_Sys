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
    public class SelfUpdateInternInfoCommandHandler : IRequestHandler<SelfUpdateInternInfoCommand, UpdateInternInfoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public SelfUpdateInternInfoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateInternInfoResponse> Handle(SelfUpdateInternInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUserId = _userContextService.GetCurrentUserId();

                var user = await _unitOfWork.UserRepository.GetByIdAsync(currentUserId);
                request.Id = (int)user.InternInfoId!;

                InternInfo existingIntern = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.Id);
                if (existingIntern == null || existingIntern.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Id");

                _mapper.Map(request, existingIntern);
                existingIntern.LastUpdatedTime = _timeService.SystemTimeNow;

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
