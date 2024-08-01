using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class CongNgheDuAnRepository : BaseRepository<CongNgheDuAn>, ICongNgheDuAnRepository
    {
        private readonly ApplicationDbContext _context;

        public CongNgheDuAnRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CongNgheDuAn>> GetAllActiveAsync()
        {
            return await _context.CongNgheDuAns
                .Where(cnda => cnda.IsActive && !cnda.IsDelete)
                .ToListAsync();
        }
        public async Task<CongNgheDuAn?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.CongNgheDuAns
                .Include(cnda => cnda.CongNghe)
                .Include(cnda => cnda.DuAn)
                .FirstOrDefaultAsync(cnda => cnda.Id == id && !cnda.IsDelete);
        }
        public async Task<bool> HasRelatedRecordsAsync(int congNgheDuAnId)
        {
            var congNgheDuAn = await _context.CongNgheDuAns
                .Include(cnda => cnda.CongNghe)
                .Include(cnda => cnda.DuAn)
                .FirstOrDefaultAsync(cnda => cnda.Id == congNgheDuAnId && !cnda.IsDelete);

            if (congNgheDuAn == null) return false;

            return (congNgheDuAn.CongNghe != null && !congNgheDuAn.CongNghe.IsDelete) ||
                   (congNgheDuAn.DuAn != null && !congNgheDuAn.DuAn.IsDelete);
        }
    }
}
