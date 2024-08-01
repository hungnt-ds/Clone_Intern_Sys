using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands
{
    public class DeleteUserDuAnValidator : AbstractValidator<DeleteUserDuAnCommand>
    {
        public DeleteUserDuAnValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteUserDuAnCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
