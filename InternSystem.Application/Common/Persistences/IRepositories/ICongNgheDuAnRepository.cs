using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ICongNgheDuAnRepository : IBaseRepository<CongNgheDuAn>
    {
        Task<CongNgheDuAn?> GetByIdWithDetailsAsync(int id);
        Task<bool> HasRelatedRecordsAsync(int congNgheDuAnId);
    }
}
