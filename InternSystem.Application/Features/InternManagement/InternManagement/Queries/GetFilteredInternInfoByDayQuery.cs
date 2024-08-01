using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetFilteredInternInfoByDayQuery : IRequest<IEnumerable<GetFilteredInternInfoByDayResponse>>
    {
        public DateTime? Day { get; set; }
    }
}
