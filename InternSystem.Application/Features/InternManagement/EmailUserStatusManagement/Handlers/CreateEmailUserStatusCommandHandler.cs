using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Handlers
{
    public class CreateEmailUserStatusCommandHandler : IRequestHandler<CreateEmailUserStatusCommand, GetDetailEmailUserStatusResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public CreateEmailUserStatusCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,
            IUserContextService userContextService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<GetDetailEmailUserStatusResponse> Handle(CreateEmailUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var status = _mapper.Map<EmailUserStatus>(request);

                var intern = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.IdNguoiNhan);
                if (intern == null || intern.IsDelete == true || intern.IsActive == false)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Intern");

                string currentUserId = _userContextService.GetCurrentUserId();

                status.IdNguoiGui = currentUserId;
                status.CreatedBy = currentUserId;
                status.LastUpdatedBy = currentUserId;
                status.CreatedTime = _timeService.SystemTimeNow;
                status.LastUpdatedTime = _timeService.SystemTimeNow;
                status.IsActive = true;
                status.IsDelete = false;
                await _unitOfWork.EmailUserStatusRepository.AddAsync(status);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<GetDetailEmailUserStatusResponse>(status);
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
