using InternSystem.Infrastructure.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;

namespace InternSystem.Infrastructure.Configurations
{
    public class QuartzSetup : IConfigureOptions<QuartzOptions>
    {
        public IConfiguration Configuration { get; }
        public QuartzSetup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(NotifyUsersDeadlinesJob));
            options.AddJob<NotifyUsersDeadlinesJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
            {
                trigger
                .ForJob(jobKey)
                //// 8AM Everymorning
                .WithCronSchedule(Configuration["Quartz:CronSchedule"]);
                // Test every 5 seconds
                //.WithCronSchedule(Configuration["Quartz:TestSchedule"]);

            });
        }
    }
}