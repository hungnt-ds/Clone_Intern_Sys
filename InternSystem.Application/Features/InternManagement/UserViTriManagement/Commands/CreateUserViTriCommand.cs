using FluentValidation;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands
{
    public class CreateUserViTriValidator : AbstractValidator<CreateUserViTriCommand>
    {
        public CreateUserViTriValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("Chưa chọn User!");
            RuleFor(m => m.IdViTri)
                .NotEmpty().WithMessage("Id Vị Trí không được để trống!")
                .GreaterThan(0).WithMessage("Id Vị Trí phải lớn hơn 0.");
        }
    }
    public class CreateUserViTriCommand : IRequest<CreateUserViTriResponse>
    {
        public string UserId { get; set; }
        public int IdViTri { get; set; }
    }
}
