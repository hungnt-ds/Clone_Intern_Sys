using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CommentManagement.Commands
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Comment để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteCommentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
