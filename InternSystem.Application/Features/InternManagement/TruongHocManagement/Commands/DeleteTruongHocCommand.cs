using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands
{

    public class DeleteTruongHocValidator : AbstractValidator<DeleteTruongHocCommand>
    {
        public DeleteTruongHocValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Trường Học để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteTruongHocCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
