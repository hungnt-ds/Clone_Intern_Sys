using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Queries
{
    public class GetCongNgheByIdValidator : AbstractValidator<GetCongNgheByIdQuery>
    {
        public GetCongNgheByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetCongNgheByIdQuery : IRequest<GetCongNgheByIdResponse>
    {
        public int Id { get; set; }
    }
}
