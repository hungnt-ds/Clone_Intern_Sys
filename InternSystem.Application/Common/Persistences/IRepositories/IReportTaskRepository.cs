using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IReportTaskRepository : IBaseRepository<ReportTask>
    {
        Task<IEnumerable<ReportTask>> GetReportTasksAsync();
        Task<ReportTask> GetReportTasksByIdAsync(int id);
    }

}
