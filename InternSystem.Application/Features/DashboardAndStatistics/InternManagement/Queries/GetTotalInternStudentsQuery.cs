using MediatR;

namespace InternSystem.Application.Features.DashboardAndStatistics.InternManagement.Queries
{
    public class GetTotalInternStudentsQuery : IRequest<int>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
