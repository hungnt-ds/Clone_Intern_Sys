using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface INhomZaloRepository : IBaseRepository<NhomZalo>
    {
        Task UpdateNhomZaloAsync(NhomZalo nhomZalo);
        Task<NhomZalo> GetNhomZalosByNameAsync(string ten);
    }
}
