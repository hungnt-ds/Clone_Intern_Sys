using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class CongNgheRepository : BaseRepository<CongNghe>, ICongNgheRepository
    {
        private readonly ApplicationDbContext _context;
        public CongNgheRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CongNghe>> GetCongNghesByTenAsync(string name)
        {
            string searchTerm = name.Trim().ToLower();

            return await _context.CongNghes
                .Where(t => t.Ten.ToLower().Trim().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<CongNghe?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.CongNghes
                .Include(c => c.CongNgheDuAns)
                .Include(c => c.CauHoiCongNghes)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);
        }

        public async Task<bool> HasRelatedRecordsAsync(int congNgheId)
        {
            return await _context.CongNghes
                .AnyAsync(c => c.Id == congNgheId &&
                               (c.CongNgheDuAns.Any(cnda => !cnda.IsDelete) || c.CauHoiCongNghes.Any(chcn => !chcn.IsDelete)));
        }

        public async Task<CongNghe?> GetCongNgheByIdAsync(int id)
        {
            return await _context.CongNghes
                .Include(cn => cn.ViTri)
                .FirstOrDefaultAsync(cn => cn.Id == id && !cn.IsDelete);
        }
    }
}
