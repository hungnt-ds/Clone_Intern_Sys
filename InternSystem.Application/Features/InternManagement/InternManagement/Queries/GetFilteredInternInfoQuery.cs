using InternSystem.Domain.Entities;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetFilteredInternInfoQuery : IRequest<IEnumerable<InternInfo>>

    {
        public int? SchoolId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
