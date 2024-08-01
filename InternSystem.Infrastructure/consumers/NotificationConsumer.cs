using InternSystem.Application.Common.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace InternSystem.Infrastructure.Consumers
{
    public class NotificationConsumer
    {
        private readonly RabbitMQHelper _rabbitMQHelper;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationConsumer> _logger;

        public NotificationConsumer(RabbitMQHelper rabbitMQHelper, IEmailService emailService, ILogger<NotificationConsumer> logger)
        {
            _rabbitMQHelper = rabbitMQHelper;
            _emailService = emailService;
            _logger = logger;
        }

        public void Start()
        {
            _rabbitMQHelper.StartDequeue(OnMessageReceived);
        }

        private async Task OnMessageReceived(NotificationTask task)
        {
            // Gửi email
            var emailList = new List<string> { task.InternEmail };
            var subject = task.TieuDe;
            var body = task.NoiDung;

            var emailSent = await _emailService.SendEmailAsync(emailList, subject, body);
            if (emailSent)
            {
                _logger.LogInformation("Notification sent for task {TaskId}.", task.TaskId);
            }
            else
            {
                _logger.LogError("Failed to send notification for task {TaskId}.", task.TaskId);
            }
        }
    }
}
