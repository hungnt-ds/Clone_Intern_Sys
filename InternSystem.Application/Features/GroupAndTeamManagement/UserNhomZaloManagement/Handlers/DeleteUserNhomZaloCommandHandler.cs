﻿using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Handlers
{
    public class DeleteUserNhomZaloCommandHandler : IRequestHandler<DeleteUserNhomZaloCommand, DeleteUserNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public DeleteUserNhomZaloCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<DeleteUserNhomZaloResponse> Handle(DeleteUserNhomZaloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUserId = _userContextService.GetCurrentUserId();

                var userNhomZalo = await _unitOfWork.UserNhomZaloRepository.GetByIdAsync(request.Id);
                if (userNhomZalo == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng trong nhóm Zalo");
                }

                userNhomZalo.IsDelete = true;
                userNhomZalo.DeletedBy = currentUserId;
                userNhomZalo.DeletedTime = DateTimeOffset.Now;

                await _unitOfWork.UserNhomZaloRepository.UpdateUserNhomZaloAsync(userNhomZalo);
                await _unitOfWork.SaveChangeAsync();

                return new DeleteUserNhomZaloResponse { IsSuccessful = true };
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
