using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class PhongVanRepository : BaseRepository<PhongVan>, IPhongVanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PhongVanRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<int> GetAllPassed()
        {
            var passedList = await _dbContext.PhongVans.Where(x => x.Rank >= 5).ToListAsync();
            return passedList.Count;
        }

        public async Task<IEnumerable<PhongVan>> GetAllPhongVan()
        {
            return await _dbContext.PhongVans.ToListAsync();
        }
        public async Task<int> GetAllInterviewed()
        {
            var lichPhongVanList = await _dbContext.LichPhongVans.ToListAsync();
            return lichPhongVanList.Count;
        }
    }
}
