using AutoMapper;
using FluentValidation;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Handlers
{

    public class CreateInternInfoHandler : IRequestHandler<CreateInternInfoCommand, CreateInternInfoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public CreateInternInfoHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateInternInfoResponse> Handle(CreateInternInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    InternInfo? existingIntern = await _unitOfWork.InternInfoRepository.GetAllAsync()
                        .ContinueWith(t => t.Result.AsQueryable()
                            .Where(b => b.UserId.Equals(request.UserId))
                            .FirstOrDefault());

                    if (existingIntern != null)
                        throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.NotUnique, "Người dùng đã có thông tin trong hệ thống");
                }

                TruongHoc? existingTruong = await _unitOfWork.TruongHocRepository.GetByIdAsync(request.IdTruong);
                if (existingTruong == null || existingTruong.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Trường Học");


                if (request.KyThucTapId.HasValue)
                {
                    KyThucTap? existingKTT = await _unitOfWork.KyThucTapRepository.GetByIdAsync(request.KyThucTapId.Value);
                    if (existingKTT == null || existingKTT.IsDelete)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Kỳ thực tập");
                }


                if (request.DuAnId.HasValue)
                {
                    DuAn? existingDA = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuAnId.Value);
                    if (existingDA == null || existingDA.IsDelete)
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Dự Án");
                }

                string currentUserId = _userContextService.GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng hiện tại");

                InternInfo newIntern = _mapper.Map<InternInfo>(request);
                newIntern.CreatedBy = currentUserId;
                newIntern.LastUpdatedBy = currentUserId;
                newIntern.LastUpdatedTime = _timeService.SystemTimeNow;
                if (newIntern.LastUpdatedTime == default)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Thời gian không hợp lệ");

                newIntern.DeletedTime = _timeService.SystemTimeNow;
                if (newIntern.DeletedTime == default)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Thời gian không hợp lệ");

                newIntern = await _unitOfWork.InternInfoRepository.AddAsync(newIntern);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<CreateInternInfoResponse>(newIntern);
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Có lỗi không mong muốn !!!");
            }
        }
    }
}
