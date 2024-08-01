using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Queries
{
    public class GetAllCommentsQuery : IRequest<IEnumerable<GetDetailCommentResponse>>
    {
    }
}
