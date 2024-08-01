using FluentValidation;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries
{
    public class GetCauHoiByIdValidator : AbstractValidator<GetCauHoiByIdQuery>
    {
        public GetCauHoiByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .GreaterThan(0);
        }
    }
    public class GetCauHoiByIdQuery : IRequest<GetCauHoiByIdResponse>
    {
        public int Id { get; set; }
    }
}
