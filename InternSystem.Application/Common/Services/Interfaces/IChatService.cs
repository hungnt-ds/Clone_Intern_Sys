namespace InternSystem.Application.Common.Services.Interfaces
{
    public interface IChatService
    {
        Task SendMessageAsync(string userId, string message);
        Task UpdateMessageAsync(string userId, string messageId, string newMessageText);
        Task DeleteMessageAsync(string userId, string messageId);
        Task NotifyMessageReceived(string userId, string senderId, string message);
        Task NotifyMessageUpdated(string userId, string messageId, string newMessageText);
        Task NotifyMessageDeleted(string userId, string messageId);
    }
}
