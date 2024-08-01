using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands
{
    public class CreateCauHoiValidator : AbstractValidator<CreateCauHoiCommand>
    {
        public CreateCauHoiValidator()
        {
            RuleFor(x => x.NoiDung)
                .NotEmpty().WithMessage("Nội dung không được bỏ trống!");
        }
    }
    public class CreateCauHoiCommand : IRequest<CreateCauHoiResponse>
    {
        public string NoiDung { get; set; }
    }
}
