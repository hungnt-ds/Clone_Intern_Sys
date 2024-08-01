using FluentValidation;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Commands
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(m => m.Content)
                .NotEmpty().WithMessage("Chưa có nội dung.");
            RuleFor(m => m.IdNguoiDuocComment)
                .NotEmpty().WithMessage("Chưa chọn người được Comment")
                .GreaterThan(0).WithMessage("Id người được Comment phải lớn hơn 0.");
        }
    }

    public class CreateCommentCommand : IRequest<GetDetailCommentResponse>
    {
        public string Content { get; set; }
        public int IdNguoiDuocComment { get; set; }
    }
}
