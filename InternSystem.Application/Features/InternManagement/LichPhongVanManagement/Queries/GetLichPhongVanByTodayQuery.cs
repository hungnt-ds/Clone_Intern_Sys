using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using InternSystem.Domain.Entities;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Queries
{
    public class GetLichPhongVanByTodayQuery : IRequest<IEnumerable<GetLichPhongVanByTodayResponse>>
    {
    }
}

