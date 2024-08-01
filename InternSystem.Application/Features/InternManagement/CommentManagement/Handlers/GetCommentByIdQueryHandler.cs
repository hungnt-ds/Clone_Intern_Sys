using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using InternSystem.Application.Features.InternManagement.CommentManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Handlers
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, GetDetailCommentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCommentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetDetailCommentResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var commentRepository = _unitOfWork.GetRepository<Comment>();
                var userRepository = _unitOfWork.UserRepository;

                var commentById = await commentRepository
                    .GetAllQueryable()
                    .Include(c => c.NguoiDuocComment)
                    .Include(c => c.NguoiComment)
                    .Where(c => c.IsActive && !c.IsDelete)
                    .FirstOrDefaultAsync(c => c.IsActive && !c.IsDelete, cancellationToken);

                if (commentById == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có bình luận."
                    );
                }

                var response = _mapper.Map<GetDetailCommentResponse>(commentById);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";

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
