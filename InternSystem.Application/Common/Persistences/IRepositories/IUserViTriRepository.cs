using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IUserViTriRepository : IBaseRepository<UserViTri>
    {
        Task<UserViTri?> GetByIdAsync(int id);

    }
}
