using InternSystem.Infrastructure.Consumers;
using Microsoft.Extensions.Hosting;

namespace InternSystem.Infrastructure
{
    public class NotificationConsumerService : BackgroundService
    {
        private readonly NotificationConsumer _notificationConsumer;

        public NotificationConsumerService(NotificationConsumer notificationConsumer)
        {
            _notificationConsumer = notificationConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _notificationConsumer.Start();
            return Task.CompletedTask;
        }
    }
}