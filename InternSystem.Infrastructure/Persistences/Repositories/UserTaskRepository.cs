using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class UserTaskRepository : BaseRepository<UserTask>, IUserTaskRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserTaskRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksAsync()
        {
            return await _applicationDbContext.UserTasks
                .Where(ut => ut.IsActive && !ut.IsDelete)
                .ToListAsync();
        }

        public Task<UserTask> GetUserTasksByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
