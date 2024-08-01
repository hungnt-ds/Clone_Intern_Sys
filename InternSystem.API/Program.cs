using System.Text;
using InternSystem.API;
using InternSystem.API.Middlewares;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.Token.Middlewares;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure;
using InternSystem.Infrastructure.Consumers;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories;
using InternSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(p =>
    p.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.ConfigureApiServices(builder.Configuration);
builder.Services.ConfigureApplicationService(builder.Configuration);
builder.Services.ConfigureInfrastructureService(builder.Configuration);
builder.Services.AddIdentity<AspNetUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Add JWT authentication


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
if (!int.TryParse(rabbitMqSettings["Port"], out int port))
{
    throw new InvalidOperationException("Invalid port number in RabbitMQ configuration.");
}
builder.Services.AddSingleton(new RabbitMQHelper(
    hostName: rabbitMqSettings["HostName"],
    username: rabbitMqSettings["UserName"],
    password: rabbitMqSettings["Password"],
    managementUrl: rabbitMqSettings["ManagementUrl"],
    port: port
));
builder.Services.AddSingleton<NotificationConsumer>();
builder.Services.AddHostedService<NotificationConsumerService>();
builder.Services.AddTransient<IEmailService, EmailService>();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "InternSystem-API",
//        Version = "v1"
//    }
//    );

//    // ??c các nh?n xét XML
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath);

//    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Description = "JWT Authorization header sử dụng scheme Bearer.",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//        Name = "Authorization",
//        Scheme = "bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
//    {
//    {
//        new OpenApiSecurityScheme
//        {
//        Reference = new OpenApiReference
//            {
//                Type = ReferenceType.SecurityScheme,
//                Id = "Bearer"
//            },
//            Scheme = "oauth2",
//            Name = "Bearer",
//            In = ParameterLocation.Header,
//        },
//        new List<string>()
//        }
//    });
//});

//var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
//builder.Services.AddSingleton(new RabbitMQHelper(
//    hostName: rabbitMqSettings["HostName"],
//    username: rabbitMqSettings["UserName"],
//    password: rabbitMqSettings["Password"],
//    managementUrl: rabbitMqSettings["ManagementUrl"]
//));
//builder.Services.AddSingleton<NotificationConsumer>();
//builder.Services.AddHostedService<NotificationConsumerService>();
//builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.MapControllers();

app.MapHub<ChatHub>("Chat-hub");

app.Run();