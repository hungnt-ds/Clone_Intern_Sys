using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Commands;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Handlers
{
    public class UpdateViTriHandler : IRequestHandler<UpdateViTriCommand, UpdateViTriResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateViTriHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateViTriResponse> Handle(UpdateViTriCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ViTri? existingViTri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.Id);
                if (existingViTri == null || existingViTri.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");


                DuAn existingDuAn = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId!);
                if (existingDuAn == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án.");

                existingViTri.LastUpdatedTime = _timeService.SystemTimeNow;
                existingViTri.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingViTri = _mapper.Map(request, existingViTri);

                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateViTriResponse>(existingViTri);
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
