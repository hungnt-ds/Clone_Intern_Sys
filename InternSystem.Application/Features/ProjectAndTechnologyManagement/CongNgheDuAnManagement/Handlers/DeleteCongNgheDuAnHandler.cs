using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Handlers
{
    public class DeleteCongNgheDuAnHandler : IRequestHandler<DeleteCongNgheDuAnCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;

        public DeleteCongNgheDuAnHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, IMapper mapper, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _mapper = mapper;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteCongNgheDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CongNgheDuAn? existingCongNgheDuAn = await _unitOfWork.CongNgheDuAnRepository.GetByIdAsync(request.Id);
                if (existingCongNgheDuAn == null || existingCongNgheDuAn.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ dự án");

                string deletedBy = _userContextService.GetCurrentUserId();
                existingCongNgheDuAn.DeletedBy = deletedBy;
                existingCongNgheDuAn.DeletedTime = _timeService.SystemTimeNow;
                existingCongNgheDuAn.IsActive = false;
                existingCongNgheDuAn.IsDelete = true;

                _unitOfWork.CongNgheDuAnRepository.Update(existingCongNgheDuAn);
                await _unitOfWork.SaveChangeAsync();

                return true;
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
