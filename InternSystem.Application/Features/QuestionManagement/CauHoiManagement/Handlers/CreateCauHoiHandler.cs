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
    public class CreateCauHoiHandler : IRequestHandler<CreateCauHoiCommand, CreateCauHoiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateCauHoiHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }
        public async Task<CreateCauHoiResponse> Handle(CreateCauHoiCommand request, CancellationToken cancellationToken)
        {            
            try
            {
                var currentUser = _userContextService.GetCurrentUserId();
                CauHoi cauHoi = _mapper.Map<CauHoi>(request);

                cauHoi.CreatedBy = currentUser;
                cauHoi.LastUpdatedBy = currentUser;
                cauHoi.CreatedTime = _timeService.SystemTimeNow;
                cauHoi.LastUpdatedTime = _timeService.SystemTimeNow;

                cauHoi = await _unitOfWork.CauHoiRepository.AddAsync(cauHoi);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateCauHoiResponse>(cauHoi);
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
