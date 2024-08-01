using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Queries
{

    public class GetCauHoiCongNgheByIdValidator : AbstractValidator<GetCauHoiCongNgheByIdQuery>
    {
        public GetCauHoiCongNgheByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .GreaterThan(0);
        }
    }
    public class GetCauHoiCongNgheByIdQuery : IRequest<GetCauHoiCongNgheByIdResponse>
    {
        public int Id { get; set; }
    }
}
