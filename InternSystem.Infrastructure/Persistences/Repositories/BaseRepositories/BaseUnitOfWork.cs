using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Infrastructure.Persistences.DBContext;

namespace InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public BaseUnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
