using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ICongNgheRepository : IBaseRepository<CongNghe>
    {
        Task<IEnumerable<CongNghe>> GetCongNghesByTenAsync(string name);
        Task<CongNghe?> GetByIdWithDetailsAsync(int id);
        Task<bool> HasRelatedRecordsAsync(int congNgheId);
        Task<CongNghe?> GetCongNgheByIdAsync(int id);
    }
}
