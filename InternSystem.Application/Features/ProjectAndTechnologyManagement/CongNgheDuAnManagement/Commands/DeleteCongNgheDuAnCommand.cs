using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands
{
    public class DeleteCongNgheDuAnValidator : AbstractValidator<DeleteCongNgheDuAnCommand>
    {
        public DeleteCongNgheDuAnValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
        }
    }

    public class DeleteCongNgheDuAnCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
