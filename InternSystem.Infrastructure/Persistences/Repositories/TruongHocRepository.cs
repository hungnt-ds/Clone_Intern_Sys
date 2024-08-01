using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class TruongHocRepository : BaseRepository<TruongHoc>, ITruongHocRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TruongHocRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<TruongHoc>> GetTruongHocsByTenAsync(string ten)
        {
            string searchTerm = ten.Trim().ToLower();

            return await _applicationDbContext.TruongHocs
                .Where(t => t.Ten.ToLower().Trim().Contains(searchTerm))
                .ToListAsync();
        }
    }
}
