using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ITaskRepository : IBaseRepository<Tasks>
    {
        Task<IEnumerable<Tasks>> GetTasksByNameAsync(string name);
        public Task<IEnumerable<Tasks>> GetTaskByDuanIdAsync(int id);
        Task<IEnumerable<Tasks>> GetTasksByMoTaAsync(string mota);
        Task<IEnumerable<Tasks>> GetTasksByNoiDungAsync(string noidung);
        Task<IEnumerable<Tasks>> GetTasksWithUpcomingDeadlinesAsync();
        Task<IEnumerable<AspNetUser>> GetUserByTaskId(int taskId);
        Task<IEnumerable<Tasks>> GetAllTasks();
    }
}
