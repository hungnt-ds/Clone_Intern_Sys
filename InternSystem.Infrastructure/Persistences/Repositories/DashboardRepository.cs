using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class DashboardRepository : BaseRepository<Dashboard>, IDashboardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DashboardRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(Dashboard dashboard)
        {
            var existingDashboard = await _dbContext.Dashboards.FindAsync(dashboard.Id);
            if (existingDashboard != null)
            {
                existingDashboard.ReceivedCV = dashboard.ReceivedCV;
                existingDashboard.Interviewed = dashboard.Interviewed;
                existingDashboard.Passed = dashboard.Passed;
                existingDashboard.Interning = dashboard.Interning;
                existingDashboard.Interned = dashboard.Interned;

                _dbContext.Dashboards.Update(existingDashboard);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
