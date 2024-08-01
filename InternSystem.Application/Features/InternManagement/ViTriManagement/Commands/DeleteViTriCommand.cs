using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Commands
{
    public class DeleteViTriValidator : AbstractValidator<DeleteViTriCommand>
    {
        public DeleteViTriValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn Vị Trí để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteViTriCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
