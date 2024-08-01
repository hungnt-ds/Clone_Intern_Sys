using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class UserDuAnRepository : BaseRepository<UserDuAn>, IUserDuAnRepository
    {
        private readonly ApplicationDbContext _context;

        public UserDuAnRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<UserDuAn>> GetAllUserDuAnAsync()
        //{
        //    return await _context.UserDuAns.ToListAsync();
        //}
    }
}