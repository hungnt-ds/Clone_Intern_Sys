using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Handlers
{
    public class CreateCongNgheHandler : IRequestHandler<CreateCongNgheCommand, CreateCongNgheResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateCongNgheResponse> Handle(CreateCongNgheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                var existingCN = _unitOfWork.CongNgheRepository.GetAllAsync().Result
                                 .FirstOrDefault(d => d.Ten.Equals(request.Ten, StringComparison.OrdinalIgnoreCase));

                if (existingCN != null && !existingCN.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Duplicate, "Công nghệ đã tồn tại");
                }

                // Khôi phục nếu xóa mềm
                if (existingCN != null && existingCN.IsDelete)
                {
                    existingCN.IsActive = true;
                    existingCN.IsDelete = false;
                    await _unitOfWork.SaveChangeAsync();
                    return _mapper.Map<CreateCongNgheResponse>(existingCN);
                }

                var newCongNghe = _mapper.Map<CongNghe>(request);
                newCongNghe.CreatedBy = currentUserId;
                newCongNghe.LastUpdatedBy = currentUserId;
                newCongNghe.CreatedTime = _timeService.SystemTimeNow;
                newCongNghe.LastUpdatedTime = _timeService.SystemTimeNow;

                newCongNghe = await _unitOfWork.CongNgheRepository.AddAsync(newCongNghe);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<CreateCongNgheResponse>(newCongNghe);
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
