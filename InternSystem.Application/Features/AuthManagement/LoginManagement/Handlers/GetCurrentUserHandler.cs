using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.AuthManagement.LoginManagement.Handlers
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public GetCurrentUserHandler(IUserContextService userContextService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                var curentUser = await _unitOfWork.UserRepository.GetByIdAsync(currentUserId);
                return _mapper.Map<GetCurrentUserResponse>(curentUser);
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
