using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class ThongBaoRepository : BaseRepository<ThongBao>, IThongBaoRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ThongBaoRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
