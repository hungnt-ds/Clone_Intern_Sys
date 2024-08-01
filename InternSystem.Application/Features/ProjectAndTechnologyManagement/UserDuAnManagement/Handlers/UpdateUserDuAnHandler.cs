using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Handlers
{
    public class UpdateUserDuAnHandler : IRequestHandler<UpdateUserDuAnCommand, UpdateUserDuAnResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public UpdateUserDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<UpdateUserDuAnResponse> Handle(UpdateUserDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UserDuAn? existingUserDA = await _unitOfWork.UserDuAnRepository.GetByIdAsync(request.Id);
                if (existingUserDA == null || existingUserDA.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                if (!request.UserId.IsNullOrEmpty())
                {
                    AspNetUser? leader = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId!);
                    if (leader == null)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy leader");
                }

                DuAn existingDA = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId!);
                if (existingDA == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                var duans = await _unitOfWork.UserDuAnRepository.GetAllAsync();
                var checkUserDuAn = duans.FirstOrDefault(lp => lp.UserId == request.UserId 
                                                        && lp.DuAnId == request.DuAnId 
                                                        && lp.IdViTri == request.IdViTri);
                if (checkUserDuAn != null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Người dùng đã có vị trí và dự án này.");
                }

                existingUserDA = _mapper.Map(request, existingUserDA);
                existingUserDA.LastUpdatedBy = _userContextService.GetCurrentUserId();

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<UpdateUserDuAnResponse>(existingUserDA);
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
