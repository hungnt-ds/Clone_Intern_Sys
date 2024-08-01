using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands
{
    public class UpdateCauHoiCongNgheValidator : AbstractValidator<UpdateCauHoiCongNgheCommand>
    {
        public UpdateCauHoiCongNgheValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Chưa chọn câu hỏi công nghệ để Update!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(x => x.IdCongNghe)
                .NotEmpty().WithMessage("Id của công nghệ không được bỏ trống!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(x => x.IdCauHoi)
                .NotEmpty().WithMessage("Id của câu hỏi không được bỏ trống!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
    public class UpdateCauHoiCongNgheCommand : IRequest<UpdateCauHoiCongNgheResponse>
    {
        public int Id { get; set; }
        public int IdCongNghe { get; set; }
        public int IdCauHoi { get; set; }
    }
}
