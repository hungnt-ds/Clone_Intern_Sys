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
    public class CreateUserViTriHandler : IRequestHandler<CreateViTriCommand, CreateViTriResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public CreateUserViTriHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateViTriResponse> Handle(CreateViTriCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDuAn = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId);
                if (existingDuAn == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                ViTri newViTri = _mapper.Map<ViTri>(request);
                newViTri.CreatedBy = _userContextService.GetCurrentUserId();
                newViTri.LastUpdatedBy = newViTri.CreatedBy;
                newViTri.CreatedTime = _timeService.SystemTimeNow;
                newViTri.LastUpdatedTime = _timeService.SystemTimeNow;
                newViTri = await _unitOfWork.ViTriRepository.AddAsync(newViTri);

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateViTriResponse>(newViTri);
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
