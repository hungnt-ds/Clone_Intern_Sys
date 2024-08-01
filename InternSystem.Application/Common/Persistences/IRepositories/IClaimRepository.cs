using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IClaimRepository : IBaseRepository<ApplicationClaim>
    {
        Task<ApplicationClaim> GetByNameAsync(string claimValue);
    }
}
