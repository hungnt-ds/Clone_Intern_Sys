using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands
{
    public class DeleteCauHoiCongNgheValidator : AbstractValidator<DeleteCauHoiCongNgheCommand>
    {
        public DeleteCauHoiCongNgheValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Chưa chọn câu hỏi công nghệ để xóa!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
    public class DeleteCauHoiCongNgheCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
