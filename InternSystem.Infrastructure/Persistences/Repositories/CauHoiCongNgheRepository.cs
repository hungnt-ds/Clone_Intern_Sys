using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class CauHoiCongNgheRepository : BaseRepository<CauHoiCongNghe>, ICauHoiCongNgheRepository
    {
        private readonly ApplicationDbContext _context;
        public CauHoiCongNgheRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> HasRelatedRecordsAsync(int cauHoiId)
        {
            return await _context.CauHoiCongNghes
                .AnyAsync(chcn => chcn.IdCauHoi == cauHoiId && chcn.IsActive && !chcn.IsDelete);
        }

        public async Task<IEnumerable<CauHoiCongNghe>> GetByCauHoiIdAsync(int cauHoiId)
        {
            return await _context.CauHoiCongNghes
                .Where(chcn => chcn.IdCauHoi == cauHoiId && chcn.IsActive && !chcn.IsDelete)
                .ToListAsync();
        } 
    }
}
