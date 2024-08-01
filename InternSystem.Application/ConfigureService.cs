using System.Reflection;
using System.Text;
using FluentValidation;
using InternSystem.Application.Common.Behaviors;
using InternSystem.Application.Common.Mapping;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Commands;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Handlers;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Commands;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Handlers;
using InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Handlers;
using InternSystem.Application.Features.InternManagement.InternManagement.Utility;
using InternSystem.Application.Features.Token.Commands;
using InternSystem.Application.Features.Token.Handlers;
using InternSystem.Application.Features.Token.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

public static class ConfigureService
{
    public static IServiceCollection ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationErrorBehaviour<,>));
        });

        PasswordGenerator.Initialize(configuration);

        services.AddSingleton<TokenValidationParameters>(provider =>
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = configuration["Jwt:Issuer"],
                ValidIssuer = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.FromMinutes(60)
            };
        });
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IRequestHandler<LoginCommand, LoginResponse>, LoginCommandHandler>();
        services.AddTransient<IRequestHandler<RefreshTokenCommand, TokenResponse>, RefreshTokenHandler>();
        services.AddTransient<IRequestHandler<ResetTokenCommand, ResetTokenResponse>, ResetTokenCommandHandler>();
        services.AddTransient<IRequestHandler<SendMessageCommand, bool>, SendMessageHandler>();

        services.AddSignalR();
        return services;
    }
}