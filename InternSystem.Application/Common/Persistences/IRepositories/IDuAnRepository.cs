using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IDuAnRepository : IBaseRepository<DuAn>
    {
        Task UpdateDuAnAsync(DuAn duAn);
        Task<IEnumerable<DuAn>> GetDuAnsByTenAsync(string name);
    }
}
