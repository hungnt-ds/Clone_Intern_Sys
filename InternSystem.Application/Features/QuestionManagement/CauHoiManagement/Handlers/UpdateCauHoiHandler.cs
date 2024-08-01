using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Handlers
{
    public class UpdateCauHoiHandler : IRequestHandler<UpdateCauHoiCommand, UpdateCauHoiResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateCauHoiHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateCauHoiResponse> Handle(UpdateCauHoiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CauHoi? existingCauHoi = await _unitOfWork.CauHoiRepository.GetByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Câu hỏi không tồn tại.");

                if (existingCauHoi.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi.");

                existingCauHoi.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingCauHoi.LastUpdatedTime = _timeService.SystemTimeNow;
               existingCauHoi = _mapper.Map(request, existingCauHoi);
               
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateCauHoiResponse>(existingCauHoi);
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
