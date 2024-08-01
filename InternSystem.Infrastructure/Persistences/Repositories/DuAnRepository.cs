using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class DuAnRepository : BaseRepository<DuAn>, IDuAnRepository
    {
        private readonly ApplicationDbContext _context;

        public DuAnRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateDuAnAsync(DuAn duAn)
        {
            _context.Entry(duAn).State = EntityState.Modified;
        }
        public async Task<IEnumerable<DuAn>> GetDuAnsByTenAsync(string name)
        {
            string searchTerm = name.Trim().ToLower();

            return await _context.DuAns
                .Where(t => t.Ten.ToLower().Trim().Contains(searchTerm) && t.IsActive && !t.IsDelete )
                .ToListAsync();
        }
    }
}
