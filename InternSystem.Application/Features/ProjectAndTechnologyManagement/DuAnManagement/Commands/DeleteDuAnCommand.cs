using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands
{
    public class DeleteDuAnValidator : AbstractValidator<DeleteDuAnCommand>
    {
        public DeleteDuAnValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteDuAnCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
