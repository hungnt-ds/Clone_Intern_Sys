using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands
{
    public class DeleteUserViTriValidator : AbstractValidator<DeleteUserViTriCommand>
    {
        public DeleteUserViTriValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn User để xóa Vị trí!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteUserViTriCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
