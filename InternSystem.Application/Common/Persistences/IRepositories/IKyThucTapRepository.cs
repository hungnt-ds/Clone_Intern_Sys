using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IKyThucTapRepository : IBaseRepository<KyThucTap>
    {
        Task<IEnumerable<KyThucTap>> GetKyThucTapsByNameAsync(string ten);
        Task<IEnumerable<KyThucTap>> GetKyThucTapByTruongHocId(int IdTruong);

        //public Task<IEnumerable<KyThucTap>> GetAllKyThucTapAsync();
    }
}
