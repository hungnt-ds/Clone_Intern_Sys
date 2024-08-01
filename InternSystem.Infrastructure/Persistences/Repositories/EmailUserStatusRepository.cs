using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class EmailUserStatusRepository : BaseRepository<EmailUserStatus>, IEmailUserStatusRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmailUserStatusRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
