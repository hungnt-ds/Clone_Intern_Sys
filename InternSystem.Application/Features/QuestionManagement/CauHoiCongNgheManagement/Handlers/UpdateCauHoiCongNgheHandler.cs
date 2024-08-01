using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Handlers
{
    public class UpdateCauHoiCongNgheHandler : IRequestHandler<UpdateCauHoiCongNgheCommand, UpdateCauHoiCongNgheResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateCauHoiCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<UpdateCauHoiCongNgheResponse> Handle(UpdateCauHoiCongNgheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                CauHoiCongNghe? cauHoiCongNghe = await _unitOfWork.CauHoiCongNgheRepository.GetByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Câu hỏi công nghệ không tồn tại hoặc không còn hoạt động.");

                CauHoi? cauHoi = await _unitOfWork.CauHoiRepository.GetByIdAsync(request.IdCauHoi)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Câu hỏi không tồn tại.");

                CongNghe? congNghe = await _unitOfWork.CongNgheRepository.GetByIdAsync(request.IdCongNghe)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Công nghệ không tồn tại.");

                // Kiểm tra xem có CauHoi nào trong danh sách đã có cùng idCongNghe và idCauHoi như request hay không
                var existingCauHoiList = await _unitOfWork.CauHoiCongNgheRepository.GetAllAsync()
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi công nghệ.");
                bool exists = existingCauHoiList.Any(ch => ch.IdCongNghe == request.IdCongNghe && ch.IdCauHoi == request.IdCauHoi);
                if (exists)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Câu hỏi công nghệ đã tồn tại với công nghệ và câu hỏi này.");
                }

                cauHoiCongNghe.LastUpdatedBy = currentUserId;
                cauHoiCongNghe = _mapper.Map(request, cauHoiCongNghe);
                cauHoiCongNghe.LastUpdatedTime = _timeService.SystemTimeNow;

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<UpdateCauHoiCongNgheResponse>(cauHoiCongNghe);
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
