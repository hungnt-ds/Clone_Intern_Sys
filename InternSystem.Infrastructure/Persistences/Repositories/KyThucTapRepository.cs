using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class KyThucTapRepository : BaseRepository<KyThucTap>, IKyThucTapRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public KyThucTapRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<KyThucTap>> GetKyThucTapsByNameAsync(string ten)
        {
            string searchTerm = ten.Trim().ToLower();

            return await _applicationDbContext.KyThucTaps
                .Include(k => k.TruongHoc)
                .Where(k => k.Ten.Trim().ToLower() == searchTerm)
                .ToListAsync();
        }

        //GetKyThucTap by IdTruong
        public async Task<IEnumerable<KyThucTap>> GetKyThucTapByTruongHocId(int IdTruong)
        {
            return await _applicationDbContext.KyThucTaps
            .Where(kythuctap => kythuctap.IdTruong == IdTruong)
            .ToListAsync();
        }


        //public async Task<IEnumerable<KyThucTap>> GetAllKyThucTapAsync()
        //{
        //    return await _applicationDbContext.KyThucTaps.ToListAsync();
        //}
    }
}
