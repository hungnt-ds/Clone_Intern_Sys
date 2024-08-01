using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Handlers
{
    internal class DeleteNhomZaloTaskHandler : IRequestHandler<DeleteNhomZaloTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public DeleteNhomZaloTaskHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(DeleteNhomZaloTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                NhomZaloTask? existNhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.GetByIdAsync(request.Id);
                if (existNhomZaloTask == null || existNhomZaloTask.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy task này trong nhóm Zalo");

                existNhomZaloTask.DeletedBy = currentUserId;
                existNhomZaloTask.DeletedTime = _timeService.SystemTimeNow;
                existNhomZaloTask.IsActive = false;
                existNhomZaloTask.IsDelete = true;
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
