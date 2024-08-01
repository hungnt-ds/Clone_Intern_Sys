using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands
{
    public class CreateCauHoiCongNgheValidator : AbstractValidator<CreateCauHoiCongNgheCommand>
    {
        public CreateCauHoiCongNgheValidator()
        {
            RuleFor(x => x.IdCauHoi)
                .NotEmpty().WithMessage("Chưa chọn Câu hỏi!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(x => x.IdCongNghe)
                .NotEmpty().WithMessage("Chưa chọn Công nghệ!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
    public class CreateCauHoiCongNgheCommand : IRequest<CreateCauHoiCongNgheResponse>
    {
        public int IdCongNghe { get; set; }
        public int IdCauHoi { get; set; }
    }
}
