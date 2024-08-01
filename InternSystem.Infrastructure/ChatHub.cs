using Microsoft.AspNetCore.SignalR;

namespace InternSystem.Infrastructure
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task UpdateMessage(string userId, string messageId, string newMessageText)
        {
            await Clients.User(userId).SendAsync("UpdateMessage", messageId, newMessageText);
        }

        public async Task DeleteMessage(string userId, string messageId)
        {
            await Clients.User(userId).SendAsync("DeleteMessage", messageId);
        }
    }
}
