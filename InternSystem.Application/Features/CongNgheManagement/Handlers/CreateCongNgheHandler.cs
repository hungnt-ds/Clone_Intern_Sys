using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.CongNgheManagement.Commands;
using InternSystem.Application.Features.CongNgheManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.CongNgheManagement.Handlers
{
    public class CreateCongNgheHandler : IRequestHandler<CreateCongNgheCommand, CreateCongNgheResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public CreateCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;

        }

        public async Task<CreateCongNgheResponse> Handle(CreateCongNgheCommand request, CancellationToken cancellationToken)
        {
            try
            {                
                var existingCongNghe = (await _unitOfWork.CongNgheRepository.GetAllAsync())
                    .FirstOrDefault(d => d.Ten.Equals(request.Ten, StringComparison.OrdinalIgnoreCase));

                if (existingCongNghe != null && !existingCongNghe.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Duplicate, "Công nghệ đã tồn tại");
                }

                // Khôi phục nếu xóa mềm
                if (existingCongNghe != null && existingCongNghe.IsDelete)
                {
                    existingCongNghe.IsActive = true;
                    existingCongNghe.IsDelete = false;
                    await _unitOfWork.SaveChangeAsync();
                    return _mapper.Map<CreateCongNgheResponse>(existingCongNghe);
                }

                var currentUser = _userContextService.GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUser))
                {
                    throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Người dùng không xác thực");
                }

                var newCongNghe = _mapper.Map<CongNghe>(request);
                newCongNghe.CreatedBy = currentUser;
                newCongNghe.LastUpdatedBy = currentUser;
                newCongNghe.CreatedTime = DateTime.UtcNow.AddHours(7);
                newCongNghe.LastUpdatedTime = DateTime.UtcNow.AddHours(7);

                await _unitOfWork.CongNgheRepository.AddAsync(newCongNghe);
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
