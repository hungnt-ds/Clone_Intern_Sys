using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands
{
    public class DeleteCongNgheValidator : AbstractValidator<DeleteCongNgheCommand>
    {
        public DeleteCongNgheValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
            RuleFor(m => m.Id)
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }

    public class DeleteCongNgheCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
