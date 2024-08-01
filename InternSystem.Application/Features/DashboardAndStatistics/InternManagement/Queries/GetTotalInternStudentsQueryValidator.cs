using FluentValidation;

namespace InternSystem.Application.Features.DashboardAndStatistics.InternManagement.Queries
{
    public class GetTotalInternStudentsQueryValidator : AbstractValidator<GetTotalInternStudentsQuery>
    {
        public GetTotalInternStudentsQueryValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("End date is required.");
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be greater than or equal to start date.");
        }
    }
}
