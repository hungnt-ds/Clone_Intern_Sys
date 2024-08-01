using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.CustomExceptions;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ClaimManagement.Commands;
using InternSystem.Application.Features.ClaimManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ClaimManagement.Handlers
{
    public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, GetClaimResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateClaimCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<GetClaimResponse> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.Id);

                if (claim == null)
                {
                    throw new NotFoundException(nameof(ApplicationClaim), request.Id);
                }

                claim = _mapper.Map(request, claim);
                var currentId = _userContextService.GetCurrentUserId();
                claim.LastUpdatedBy = currentId;
                claim.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<GetClaimResponse>(claim);
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
