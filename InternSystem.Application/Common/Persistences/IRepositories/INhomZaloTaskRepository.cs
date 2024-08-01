using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface INhomZaloTaskRepository : IBaseRepository<NhomZaloTask>
    {
        Task<IEnumerable<NhomZaloTask>> GetTaskByNhomZaloIdAsync(int id);
        public Task<NhomZaloTask> GetByNhomZaloIdAndTaskIdAsync(int nhomZaloid, int taskId);
    }
}
