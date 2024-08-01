using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands
{
    public class DeleteCauHoiValidator : AbstractValidator<DeleteCauHoiCommand>
    {
        public DeleteCauHoiValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Chưa chọn câu hỏi để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
    public class DeleteCauHoiCommand : IRequest<bool>
    {
        public int Id { get; set; }

    }
}
