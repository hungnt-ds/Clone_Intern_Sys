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
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Handlers
{
    public class UpdateUserViTriHandler : IRequestHandler<UpdateUserViTriCommand, UpdateUserViTriResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public UpdateUserViTriHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<UpdateUserViTriResponse> Handle(UpdateUserViTriCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UserViTri? existingUserViTri = await _unitOfWork.UserViTriRepository.GetByIdAsync(request.Id);
                if (existingUserViTri == null || existingUserViTri.IsDelete) 
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng ở vị trí này");

                if (!request.UserId.IsNullOrEmpty())
                {
                    AspNetUser existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId!);
                    if (existingUser == null)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }

                ViTri existingViTri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.IdViTri!);
                if (existingViTri == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");

                request.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingUserViTri = _mapper.Map(request, existingUserViTri);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateUserViTriResponse>(existingUserViTri);
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
