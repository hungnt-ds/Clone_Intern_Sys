using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class ReportTaskRepository : BaseRepository<ReportTask>, IReportTaskRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ReportTaskRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<ReportTask>> GetReportTasksAsync()
        {
            return await _applicationDbContext.ReportTasks
                  .Where(rt => rt.IsActive && !rt.IsDelete)
                  .ToListAsync();
        }

        public Task<ReportTask> GetReportTasksByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
