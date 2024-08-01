using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IUserTaskRepository : IBaseRepository<UserTask>
    {
        Task<IEnumerable<UserTask>> GetUserTasksAsync();
        Task<UserTask> GetUserTasksByIdAsync(int id);
    }

}
