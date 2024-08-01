using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Handlers
{
    public class CreateCongNgheDuAnHandler : IRequestHandler<CreateCongNgheDuAnCommand, CreateCongNgheDuAnResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateCongNgheDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateCongNgheDuAnResponse> Handle(CreateCongNgheDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();
                var systemTimeNow = _timeService.SystemTimeNow;

                CongNghe? existingCN = await _unitOfWork.CongNgheRepository.GetByIdAsync(request.IdCongNghe);
                if (existingCN == null || existingCN.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ");

                DuAn? existingDA = await _unitOfWork.DuAnRepository.GetByIdAsync(request.IdDuAn);
                if (existingDA == null || existingDA.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                CongNgheDuAn newCNDA = _mapper.Map<CongNgheDuAn>(request);
                newCNDA.LastUpdatedBy = currentUserId;
                newCNDA.CreatedBy = currentUserId;
                newCNDA.CreatedTime = systemTimeNow;
                newCNDA.LastUpdatedTime = systemTimeNow;
                newCNDA = await _unitOfWork.CongNgheDuAnRepository.AddAsync(newCNDA);

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateCongNgheDuAnResponse>(newCNDA);
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