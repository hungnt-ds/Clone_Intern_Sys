using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Handlers
{
    public class UpdateDuAnHandler : IRequestHandler<UpdateDuAnCommand, UpdateDuAnResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }
        public async Task<UpdateDuAnResponse> Handle(UpdateDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                DuAn? existingDuAn = await _unitOfWork.DuAnRepository.GetByIdAsync(request.Id);
                if (existingDuAn == null || existingDuAn.IsDelete == true)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");
                }

                var listDuAn = await _unitOfWork.DuAnRepository.GetAllAsync();
                if (listDuAn.Any(da => da.Ten.Equals(request.Ten, StringComparison.OrdinalIgnoreCase) && da.Id != request.Id))
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.NotUnique, "Dự án đã tồn tại.");
                }

                if (!request.LeaderId.IsNullOrEmpty())
                {
                    AspNetUser? leader = await _unitOfWork.UserRepository.GetByIdAsync(request.LeaderId!);
                    if (leader == null)
                    {
                        throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy Leader");
                    }
                }

                var updateBy = _userContextService.GetCurrentUserId();
                if (updateBy.IsNullOrEmpty())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy Id của người dùng hiện tại");
                }

                if (request.ThoiGianBatDau > request.ThoiGianKetThuc || request.ThoiGianBatDau == request.ThoiGianKetThuc)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Thời gian bắt đầu phải là trước Thời gian kết thúc");
                }

                existingDuAn = _mapper.Map(request, existingDuAn);
                existingDuAn.LastUpdatedBy = updateBy;
                existingDuAn.LastUpdatedTime = _timeService.SystemTimeNow;

                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateDuAnResponse>(existingDuAn);
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
