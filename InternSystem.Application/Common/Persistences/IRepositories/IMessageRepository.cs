using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Common.Persistences.IRepositories
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<List<Message>> GetMessagesAsync(string senderId, string receiverId);
        Task UpdateMessageAsync(Message message);
    }
}
