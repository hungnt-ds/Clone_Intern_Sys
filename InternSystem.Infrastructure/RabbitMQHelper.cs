using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InternSystem.Infrastructure
{
    public class RabbitMQHelper
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName = "notificationQueue";
        private readonly string _managementUrl;
        private readonly HttpClient _httpClient;

        public RabbitMQHelper(string hostName, string username, string password, string managementUrl, int port)
        {
            _factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = username,
                Password = password,
                Port = 5680
            };

            _managementUrl = managementUrl;
            _httpClient = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public void EnqueueNotification(NotificationTask task)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = JsonSerializer.Serialize(task);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: properties,
                                 body: body);
        }

        public void StartDequeue(Func<NotificationTask, Task> onMessageReceived)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var task = JsonSerializer.Deserialize<NotificationTask>(message);

                await onMessageReceived(task);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public async Task<string> GetQueueInfoAsync(string queueName)
        {
            var response = await _httpClient.GetAsync($"{_managementUrl}/api/queues/%2f/{queueName}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

    public class NotificationTask
    {
        public int TaskId { get; set; }
        public string IdNguoiNhan { get; set; }
        public string IdNguoiGui { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string InternEmail { get; set; }
        public DateTime Deadline { get; set; }
    }
}
