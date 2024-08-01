using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Handlers
{
    public class UpdateUserNhomZaloCommandHandler : IRequestHandler<UpdateUserNhomZaloCommand, UpdateUserNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;


        public UpdateUserNhomZaloCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateUserNhomZaloResponse> Handle(UpdateUserNhomZaloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUserId = _userContextService.GetCurrentUserId();
                var userNhomZalo = await _unitOfWork.UserNhomZaloRepository.GetByIdAsync(request.Id);
                if (userNhomZalo == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng trong nhóm Zalo.");
                }
                UserNhomZalo a = new();
                _mapper.Map(request, userNhomZalo);
                userNhomZalo.LastUpdatedBy = currentUserId;
                userNhomZalo.LastUpdatedTime = _timeService.SystemTimeNow;

                _unitOfWork.UserNhomZaloRepository.UpdateUserNhomZaloAsync(userNhomZalo);
                await _unitOfWork.SaveChangeAsync();
                return new UpdateUserNhomZaloResponse { Id = userNhomZalo.Id };
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi");
            }
        }
    }
}