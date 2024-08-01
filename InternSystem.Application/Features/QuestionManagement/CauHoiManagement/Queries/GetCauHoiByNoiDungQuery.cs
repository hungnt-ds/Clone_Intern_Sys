using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using MediatR;


namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries
{
    public class GetCauHoiByNoiDungQueryValidator : AbstractValidator<GetCauHoiByNoiDungQuery>
    {
        public GetCauHoiByNoiDungQueryValidator()
        {
            RuleFor(model => model.noidung).NotEmpty().WithMessage("Cần nhập nội dung");
        }
    }

    public class GetCauHoiByNoiDungQuery : IRequest<IEnumerable<GetCauHoiByNoiDungResponse>>
    {
        public string noidung { get; set; } = string.Empty;
    }
}
