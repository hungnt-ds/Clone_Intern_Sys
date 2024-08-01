using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class NhomZaloRepository : BaseRepository<NhomZalo>, INhomZaloRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NhomZaloRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task UpdateNhomZaloAsync(NhomZalo nhomZalo)
        {
            _applicationDbContext.Entry(nhomZalo).State = EntityState.Modified;

        }
        public async Task<NhomZalo> GetNhomZalosByNameAsync(string name)
        {
            string searchTerm = name.ToLower().Trim().ToLower();

            return await _applicationDbContext.NhomZalos.FirstOrDefaultAsync(t => t.TenNhom.ToLower().Trim().Contains(searchTerm));
        }
    }
}
