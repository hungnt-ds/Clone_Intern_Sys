using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public MessageRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<Message>> GetMessagesAsync(string senderId, string receiverId)
        {
            return await _applicationDbContext.Messages
                .Where(m => (m.IdSender == senderId && m.IdReceiver == receiverId) ||
                            (m.IdSender == receiverId && m.IdReceiver == senderId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
        public async Task UpdateMessageAsync(Message message)
        {
            _applicationDbContext.Entry(message).State = EntityState.Modified;

        }



    }
}
