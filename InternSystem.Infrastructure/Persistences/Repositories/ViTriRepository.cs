using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class ViTriRepository : BaseRepository<ViTri>, IViTriRepository
    {
        private readonly ApplicationDbContext _context;

        public ViTriRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ViTri>> GetVitrisByNameAsync(string name)
        {
            string searchTerm = name.Trim().ToLower();

            return await _context.ViTris
                .Where(t => t.Ten.ToLower().Trim().Contains(searchTerm))
                .ToListAsync();
        }
    }
}
