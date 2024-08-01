using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class UserNhomZaloRepository : BaseRepository<UserNhomZalo>, IUserNhomZaloRepository
    {
        private ApplicationDbContext _dbContext;

        public UserNhomZaloRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            {
                _dbContext = dbContext;
            }
        }

        public async Task<UserNhomZalo> GetByUserIdAndNhomZaloIdAsync(string userId, int nhomZaloId)
        {
            return await _dbContext.UserNhomZalos
                .FirstOrDefaultAsync(u => u.UserId == userId &&
                                          (u.IdNhomZaloChung == nhomZaloId || u.IdNhomZaloRieng == nhomZaloId));
        }

        public async Task<IEnumerable<UserNhomZalo>> GetByUserIdAsync(string userid)
        {
            var data = await _dbContext.UserNhomZalos
                                       .Where(u => u.UserId == userid)
                                       .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<UserNhomZalo>> GetByNhomZaloIdAsync(int nhomZaloId)
        {
            var data = await _dbContext.UserNhomZalos
                                        .Where(u => u.IdNhomZaloRieng == nhomZaloId || u.IdNhomZaloChung == nhomZaloId)
                                        .ToListAsync();
            return data;
        }

        public async Task UpdateUserNhomZaloAsync(UserNhomZalo userNhomZalo)
        {
            _dbContext.Entry(userNhomZalo).State = EntityState.Modified;

        }
    }
}