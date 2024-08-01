using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ILichPhongVanRepository : IBaseRepository<LichPhongVan>
    {
        Task<IEnumerable<LichPhongVan>> GetLichPhongVanByToday();
        Task<IEnumerable<LichPhongVan>> GetAllLichPhongVan();
        Task UpdateAsync(LichPhongVan updatedLPV);
        Task<bool> IsNguoiPhongVanConflict(string interviewerId, DateTime interviewTime);
        Task<bool> IsNguoiDuocPhongVanConflict(int intervieweeId, DateTime interviewTime);
    }
}
