using InternSystem.Application.Common.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace InternSystem.Infrastructure.Services
{
    internal class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task UpdateMessageAsync(string userId, string messageId, string newMessageText)
        {
            await _hubContext.Clients.User(userId).SendAsync("UpdateMessage", messageId, newMessageText);
        }

        public async Task DeleteMessageAsync(string userId, string messageId)
        {
            await _hubContext.Clients.User(userId).SendAsync("DeleteMessage", messageId);
        }

        public async Task NotifyMessageReceived(string userId, string senderId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", senderId, message);
        }

        public async Task NotifyMessageUpdated(string userId, string messageId, string newMessageText)
        {
            await _hubContext.Clients.User(userId).SendAsync("UpdateMessage", messageId, newMessageText);
        }

        public async Task NotifyMessageDeleted(string userId, string messageId)
        {
            await _hubContext.Clients.User(userId).SendAsync("DeleteMessage", messageId);
        }
    }
}