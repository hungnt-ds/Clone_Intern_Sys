using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using InternSystem.Application.Features.InternManagement.CommentManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Handlers
{
    public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, IEnumerable<GetDetailCommentResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCommentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetDetailCommentResponse>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var commentRepository = _unitOfWork.GetRepository<Comment>();
                var userRepository = _unitOfWork.UserRepository;

                var listComment = await commentRepository
                    .GetAllQueryable()
                    .Include(c => c.NguoiDuocComment)
                    .Include(c => c.NguoiComment)
                    .Where(c => c.IsActive && !c.IsDelete)
                    .ToListAsync(cancellationToken);

                if (listComment == null || !listComment.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có bình luận."
                    );
                }

                var response = _mapper.Map<IEnumerable<GetDetailCommentResponse>>(listComment);

                foreach (var commentResponse in response)
                {
                    commentResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(commentResponse.CreatedBy) ?? "Người dùng không xác định";
                    commentResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(commentResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                return response;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
