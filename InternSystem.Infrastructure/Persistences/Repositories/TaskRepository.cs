using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class TaskRepository : BaseRepository<Tasks>, ITaskRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TaskRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Tasks>> GetTasksByNameAsync(string task)
        {
            string searchTerm = task.Trim().ToLower();

            return await _applicationDbContext.Tasks
                .Where(t => t.MoTa.ToLower().Trim().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTaskByDuanIdAsync(int id)
        {
            return await _applicationDbContext.Tasks
                .Where(t => t.DuAnId == id).ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTasksWithUpcomingDeadlinesAsync()
        {
            var tomorrow = DateTime.UtcNow.AddDays(1);
            return await _applicationDbContext.Tasks.Where(t => !t.HoanThanh && t.HanHoanThanh.Date == tomorrow.Date).ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTasksByMoTaAsync(string mota)
        {
            return await _applicationDbContext.Tasks
                .Where(t => t.MoTa.ToLower().Trim().Contains(mota.Trim().ToLower()) && t.IsActive == true && t.IsDelete == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTasksByNoiDungAsync(string noidung)
        {
            return await _applicationDbContext.Tasks
              .Where(t => t.NoiDung.ToLower().Trim().Contains(noidung.Trim().ToLower()) && t.IsActive == true && t.IsDelete == false)
              .ToListAsync();
        }

        public async Task<IEnumerable<AspNetUser>> GetUserByTaskId(int taskId)
        {
            return await _applicationDbContext.UserTasks
            .Where(ut => ut.TaskId == taskId)
            .Include(ut => ut.User)
            .Select(ut => ut.User)
            .ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetAllTasks()
        {
            return await _applicationDbContext.Tasks
                .Where(t => t.IsActive && !t.IsDelete)
                .ToListAsync();
        }
    }
}
