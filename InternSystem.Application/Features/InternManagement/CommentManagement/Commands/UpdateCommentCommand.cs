using FluentValidation;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Commands
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(m => m.Content)
                .NotEmpty().WithMessage("Chưa có nội dung.");
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Comment để Update!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
    public class UpdateCommentCommand : IRequest<GetDetailCommentResponse>
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
