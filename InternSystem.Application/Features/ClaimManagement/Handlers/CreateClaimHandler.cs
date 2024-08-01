using AutoMapper;
using InternSystem.Application.Common.Constants;
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
    public class CreateClaimHandler : IRequestHandler<AddClaimCommand, GetClaimResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateClaimHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<GetClaimResponse> Handle(AddClaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newClaim = _mapper.Map<ApplicationClaim>(request);
                var check = await _unitOfWork.ClaimRepository.GetByNameAsync(request.Value);
                if (check != null)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "Giá trị Claim đã được tạo.");
                }
                var currentId = _userContextService.GetCurrentUserId();
                newClaim.CreatedBy = currentId;
                newClaim.LastUpdatedBy = currentId;
                newClaim.LastUpdatedTime = _timeService.SystemTimeNow;
                newClaim.CreatedTime = _timeService.SystemTimeNow;
                newClaim = await _unitOfWork.ClaimRepository.AddAsync(newClaim);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<GetClaimResponse>(newClaim);
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
