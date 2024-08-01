using FluentValidation;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Queries
{
    public class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
    {
        public GetCommentByIdQueryValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetCommentByIdQuery : IRequest<GetDetailCommentResponse>
    {
        public int Id { get; set; }
    }
}
