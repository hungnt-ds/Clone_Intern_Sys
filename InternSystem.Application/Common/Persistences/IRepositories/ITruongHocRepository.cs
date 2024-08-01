using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ITruongHocRepository : IBaseRepository<TruongHoc>
    {
        Task<IEnumerable<TruongHoc>> GetTruongHocsByTenAsync(string ten);
    }
}
