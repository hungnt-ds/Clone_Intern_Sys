using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class CauHoiRepository : BaseRepository<CauHoi>, ICauHoiRepository
    {
        private readonly ApplicationDbContext _context;
        public CauHoiRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CauHoi>> GetCauHoiByNoiDungAsync(string noidung)
        {
            string searchTerm = noidung.Trim().ToLower();
            return await _context.CauHois
                .Where(ch => ch.NoiDung.ToLower().Trim().Contains(noidung.Trim().ToLower()))
                .ToListAsync();
        }

        public async Task<bool> HasRelatedRecordsAsync(int cauHoiId)
        {
            return await _context.CauHoiCongNghes
                .AnyAsync(chcn => chcn.IdCauHoi == cauHoiId && chcn.IsActive && !chcn.IsDelete);
        }
    }
}
