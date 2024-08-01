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
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, GetDetailCommentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;

        public UpdateCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserContextService userContextService, ITimeService timeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<GetDetailCommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Comment? existComment = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id)
                                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy comment");

                if (existComment.IsDelete || !existComment.IsActive)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy comment");
                }

                _mapper.Map(request, existComment);

                existComment.LastUpdatedTime = _timeService.SystemTimeNow;
                existComment.LastUpdatedBy = _userContextService.GetCurrentUserId();
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<GetDetailCommentResponse>(existComment);

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
