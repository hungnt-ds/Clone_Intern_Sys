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
    public class UpdateCongNgheDuAnHandler : IRequestHandler<UpdateCongNgheDuAnCommand, UpdateCongNgheDuAnResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public UpdateCongNgheDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateCongNgheDuAnResponse> Handle(UpdateCongNgheDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CongNghe? existingCN = await _unitOfWork.CongNgheRepository.GetByIdAsync(request.IdCongNghe);
                if (existingCN == null || existingCN.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ");

                DuAn? existingDA = await _unitOfWork.DuAnRepository.GetByIdAsync(request.IdDuAn);
                if (existingDA == null || existingDA.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                CongNgheDuAn? existingCNDA = await _unitOfWork.CongNgheDuAnRepository.GetByIdAsync(request.Id);
                if (existingCNDA == null || existingCNDA.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ cho dự án");

                existingCNDA.LastUpdatedBy = _userContextService.GetCurrentUserId();
                existingCNDA.LastUpdatedTime = _timeService.SystemTimeNow;

                _mapper.Map(request, existingCNDA);

                _unitOfWork.CongNgheDuAnRepository.Update(existingCNDA);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<UpdateCongNgheDuAnResponse>(existingCNDA);
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
