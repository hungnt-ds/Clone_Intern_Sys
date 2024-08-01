using InternSystem.Application.Common.Persistences.IRepositories;
using Microsoft.Extensions.Logging;
using Quartz;

namespace InternSystem.Infrastructure.Jobs
{
    public class NotifyUsersDeadlinesJob : IJob
    {
        private readonly RabbitMQHelper _rabbitMQHelper;
        private readonly ILogger<NotifyUsersDeadlinesJob> _logger;
        public readonly IUnitOfWork _unitOfWork;
        public NotifyUsersDeadlinesJob(ILogger<NotifyUsersDeadlinesJob> logger, IUnitOfWork unitOfWork, RabbitMQHelper rabbitMQHelper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _rabbitMQHelper = rabbitMQHelper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("NotifyUsersJob started at {time}", DateTimeOffset.Now);

            try
            {
                var tasks = await _unitOfWork.TaskRepository.GetTasksWithUpcomingDeadlinesAsync();

                if (tasks == null || !tasks.Any())
                {
                    _logger.LogInformation("No tasks with upcoming deadlines found.");
                    return;
                }

                foreach (var task in tasks)
                {
                    _logger.LogInformation("Processing task {TaskId} with description '{TaskDescription}' and deadline {TaskDeadline}",
                        task.Id, task.MoTa, task.HanHoanThanh);

                    var users = await _unitOfWork.TaskRepository.GetUserByTaskId(task.Id);

                    if (users == null || !users.Any())
                    {
                        _logger.LogInformation("No users assigned to task {TaskId}.", task.Id);
                        continue;
                    }

                    foreach (var user in users)
                    {
                        _logger.LogInformation("Notifying user {UserId} with email {UserEmail} about task {TaskId}.", user.Id, user.Email, task.Id);

                        var notificationTask = new NotificationTask
                        {
                            TaskId = task.Id,
                            IdNguoiNhan = user.Id,
                            IdNguoiGui = task.CreatedBy,
                            TieuDe = $"Task {task.Id} is nearing its deadline",
                            NoiDung = $"The task '{task.MoTa}' is due on {task.HanHoanThanh:yyyy-MM-dd}. Please make sure to complete it.",
                            InternEmail = user.Email,
                            Deadline = task.HanHoanThanh
                        };

                        _rabbitMQHelper.EnqueueNotification(notificationTask);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing NotifyUsersJob.");
            }

            _logger.LogInformation("NotifyUsersJob completed at {time}", DateTimeOffset.Now);

        }
    }
}
