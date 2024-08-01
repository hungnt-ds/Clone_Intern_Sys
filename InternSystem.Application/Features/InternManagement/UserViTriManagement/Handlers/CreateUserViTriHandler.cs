using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Handlers
{
    public class CreateUserViTriHandler : IRequestHandler<CreateUserViTriCommand, CreateUserViTriResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;


        public CreateUserViTriHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateUserViTriResponse> Handle(CreateUserViTriCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ViTri? existingViTri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.IdViTri);

                if (existingViTri == null) return new CreateUserViTriResponse() { Errors = "ViTri not found" };

                AspNetUser? existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (existingUser == null) return new CreateUserViTriResponse() { Errors = "User not found" };

                var listUserVitris = await _unitOfWork.UserViTriRepository.GetAllAsync();
                var matchingUsers = listUserVitris.Any(lp => lp.UserId == request.UserId);
                if (matchingUsers)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người dùng đã có vị trí.");
                }

                UserViTri newUserViTri = _mapper.Map<UserViTri>(request);
                newUserViTri.CreatedBy = _userContextService.GetCurrentUserId();
                newUserViTri.LastUpdatedBy = newUserViTri.CreatedBy;
                newUserViTri.CreatedTime = _timeService.SystemTimeNow;
                newUserViTri.LastUpdatedTime = _timeService.SystemTimeNow;
                newUserViTri.IsActive = true;
                newUserViTri.IsDelete = false;
                newUserViTri = await _unitOfWork.UserViTriRepository.AddAsync(newUserViTri);

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateUserViTriResponse>(newUserViTri);
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
