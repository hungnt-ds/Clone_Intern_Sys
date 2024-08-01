using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IUserNhomZaloRepository : IBaseRepository<UserNhomZalo>
    {
        Task<UserNhomZalo> GetByUserIdAndNhomZaloIdAsync(string userId, int nhomZaloId);
        Task UpdateUserNhomZaloAsync(UserNhomZalo userNhomZalo);
        Task<IEnumerable<UserNhomZalo>> GetByNhomZaloIdAsync(int nhomZaloId);
        Task<IEnumerable<UserNhomZalo>> GetByUserIdAsync(string userId);
    }
}
