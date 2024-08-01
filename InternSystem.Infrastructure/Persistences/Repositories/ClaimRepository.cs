using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class ClaimRepository : BaseRepository<ApplicationClaim>, IClaimRepository
    {
        private readonly ApplicationDbContext _context;
        public ClaimRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApplicationClaim> GetByNameAsync(string claimValue)
        {
            return await _context.ApplicationClaim.FirstOrDefaultAsync(c => c.Value == claimValue);
        }
    }
}
