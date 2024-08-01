using FluentValidation;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetInternStatsBySchoolIdQueryValidator : AbstractValidator<GetInternStatsBySchoolIdQuery>
    {
        public GetInternStatsBySchoolIdQueryValidator()
        {
            RuleFor(q => q.SchoolId)
            .GreaterThan(0)
            .WithMessage("Vui lòng chọn trường học."); 
        }
    }

    public class GetInternStatsBySchoolIdQuery : IRequest<List<GetInternStatsBySchoolIdResponse>>
    {
        public int SchoolId { get; set; }
    }
}
