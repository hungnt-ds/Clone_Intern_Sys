using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IDashboardRepository : IBaseRepository<Dashboard>
    {
        Task UpdateAsync(Dashboard dashboard);
    }
}
