using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface ICauHoiRepository : IBaseRepository<CauHoi>
    {
        Task<IEnumerable<CauHoi>> GetCauHoiByNoiDungAsync(string noidung);
        Task<bool> HasRelatedRecordsAsync(int cauHoiId);
    }
}
