using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class NhomZaloTaskRepository : BaseRepository<NhomZaloTask>, INhomZaloTaskRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NhomZaloTaskRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<NhomZaloTask>> GetTaskByNhomZaloIdAsync(int id)
        {
            return await _applicationDbContext.NhomZaloTasks
                .Where(t => t.Id == id).ToListAsync();


        }

        public async Task<NhomZaloTask?> GetByNhomZaloIdAndTaskIdAsync(int nhomZaloid, int taskId)
        {
            return await _applicationDbContext.NhomZaloTasks
                .FirstOrDefaultAsync(u => u.NhomZaloId == nhomZaloid && u.TaskId == taskId);
        }
    }
}
