using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IPhongVanRepository : IBaseRepository<PhongVan>
    {
        Task<IEnumerable<PhongVan>> GetAllPhongVan();
        Task<int> GetAllPassed();
        Task<int> GetAllInterviewed();
    }
}
