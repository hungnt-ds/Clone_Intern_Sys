using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ICauHoiCongNgheRepository : IBaseRepository<CauHoiCongNghe>
    {
        Task<IEnumerable<CauHoiCongNghe>> GetByCauHoiIdAsync(int cauHoiId);
    }
}
