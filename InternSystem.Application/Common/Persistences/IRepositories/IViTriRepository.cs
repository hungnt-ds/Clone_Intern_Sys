using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IViTriRepository : IBaseRepository<ViTri>
    {
        Task<IEnumerable<ViTri>> GetVitrisByNameAsync(string name);
    }
}
