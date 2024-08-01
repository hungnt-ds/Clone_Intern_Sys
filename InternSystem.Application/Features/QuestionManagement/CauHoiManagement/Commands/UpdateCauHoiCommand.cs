using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands
{
    public class UpdateCauHoiValidator : AbstractValidator<UpdateCauHoiCommand>
    {
        public UpdateCauHoiValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Chưa chọn câu hỏi để Update!")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(x => x.NoiDung)
                .NotEmpty().WithMessage("Nội dung không được bỏ trống!");
        }
    }
    public class UpdateCauHoiCommand : IRequest<UpdateCauHoiResponse>
    {
        public int Id { get; set; }
        public string NoiDung { get; set; }
    }
}
