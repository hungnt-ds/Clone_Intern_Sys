using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CommentManagement.Commands;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Handlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, GetDetailCommentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;


        public CreateCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<GetDetailCommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUserId = _userContextService.GetCurrentUserId();

                var intern = await _unitOfWork.InternInfoRepository.GetByIdAsync(request.IdNguoiDuocComment)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thực tập sinh");
                if (intern.IsDelete || !intern.IsActive)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy thực tập sinh");
                }

                var comment = _mapper.Map<Comment>(request);
                comment.IdNguoiComment = currentUserId;
                comment.CreatedBy = currentUserId;
                comment.LastUpdatedBy = currentUserId;
                comment.CreatedTime = _timeService.SystemTimeNow;
                comment.LastUpdatedTime = _timeService.SystemTimeNow;

                await _unitOfWork.CommentRepository.AddAsync(comment);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<GetDetailCommentResponse>(comment);
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
