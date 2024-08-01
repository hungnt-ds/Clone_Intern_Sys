using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Handlers
{
    public class InternToLeaderHandler : IRequestHandler<PromoteMemberToLeaderCommand, ExampleResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public InternToLeaderHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<ExampleResponse> Handle(PromoteMemberToLeaderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                // Tìm nhóm Zalo cần cập nhật leader
                var existingNhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.NhomZaloId);
                if (existingNhomZalo == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo");
                }

                // Kiểm tra 'người dùng' là 'mentor' của 'nhóm Zalo' mới được phép cập nhật
                //var mentor = await _unitOfWork.UserNhomZaloRepository.GetByUserIdAndNhomZaloIdAsync(_userContextService.GetCurrentUserId(), existingNhomZalo.Id);
                //if (mentor == null || !mentor.IsMentor || mentor.IsDelete == true)
                //{
                //    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Người dùng chưa phải là mentor");
                //}

                // Kiểm tra người dùng có tồn tại hay không
                var member = await _unitOfWork.UserNhomZaloRepository.GetByUserIdAndNhomZaloIdAsync(request.MemberId, existingNhomZalo.Id);
                if (member == null || member.IsDelete == true)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }

                // Tìm task của nhóm zalo cần cập nhật
                //var taskOfNhomZalo = await _unitOfWork.NhomZaloTaskRepository.GetTaskByNhomZaloIdAsync(existingNhomZalo.Id);
                //foreach (var item in taskOfNhomZalo)
                //{
                //    // lấy task từ NhomZaloTask
                //    var tasks = await _unitOfWork.TaskRepository.GetByIdAsync(item.TaskId);

                //    // so sánh với dự án muốn cập nhật
                //    if (tasks == null || tasks.DuAnId != request.DuanId)
                //    {
                //        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Dự án không phù hợp với nhóm Zalo");
                //    }
                //}

                member.IsLeader = true;
                member.LastUpdatedBy = currentUserId;
                member.LastUpdatedTime = _timeService.SystemTimeNow;
                await _unitOfWork.UserNhomZaloRepository.UpdateUserNhomZaloAsync(member);

                // cập nhật leader cho dự án
                //var projectToUpdate = await _unitOfWork.DuAnRepository.GetByIdAsync(request.DuanId);
                //if (projectToUpdate == null || projectToUpdate.IsDelete == true || projectToUpdate.ThoiGianKetThuc < _timeService.SystemTimeNow)
                //{
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");
                //}
                //projectToUpdate.LeaderId = request.MemberId;
                //projectToUpdate.LastUpdatedBy = currentUserId;
                //projectToUpdate.LastUpdatedTime = _timeService.SystemTimeNow;
                //await _unitOfWork.DuAnRepository.UpdateDuAnAsync(projectToUpdate);

                await _unitOfWork.SaveChangeAsync();
                var response = new ExampleResponse
                {
                    NhomZaloId = existingNhomZalo.Id,
                    Leader = member.UserId,
                    //Mentor = mentor.UserId
                };
                return response;
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
