// See https://aka.ms/new-console-template for more information

using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Configurations;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories;
using InternSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Text.Json.Serialization;

public static class ConfigureService
{
    public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddMvc().AddJsonOptions(options =>
        //{
        //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        //    options.JsonSerializerOptions.MaxDepth = 0; // Set your desired depth (or 0 for unlimited).
        //});

        services.AddIdentityCore<AspNetUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityCore<AspNetUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<ITimeService, TimeService>();
        services.AddTransient<IFileService, FileService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IMediatorService, MediatorService>();

        //Quartz
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        services.ConfigureOptions<QuartzSetup>();

        return services;
    }
}