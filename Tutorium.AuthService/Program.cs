using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Tutorium.AuthService.Core.Abstractions;
using Tutorium.AuthService.Core.Models.Google;
using Tutorium.AuthService.Core.Models.JwtToken;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.UseCase;
using Tutorium.AuthService.Core.Services;
using Tutorium.AuthService.Core.Services.Interfaces;
using Tutorium.AuthService.Grpc.Clients;
using Tutorium.AuthService.Infrastructure.Jwt;
using Tutorium.AuthService.Infrastructure.Middleware;
using Tutorium.AuthService.Infrastructure.Postgres;
using Tutorium.AuthService.Infrastructure.Redis;
using Tutorium.Shared.Utils.EntityFramework.Base;
using Tutorium.Shared.Utils.Grpc;
using static Tutorium.Grpc.Notification.NotificationGrpc;
using static Tutorium.Grpc.User.UserGrpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });

    options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "https://localhost:8000", "https://localhost:8000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

ConfigureAppSettings(builder);
ConfigureServices(builder);

var app = builder.Build();

ConfigureApp(app);

app.Run();

#region Setup Helpers

void ConfigureAppSettings(WebApplicationBuilder builder)
{
    builder.Configuration
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

    if (builder.Environment.IsDevelopment())
        builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
    
    builder.Configuration.AddEnvironmentVariables();
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddRedisModule(builder.Configuration)
        .AddPostgresModule(builder.Configuration);
    
    builder.Services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();
    builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
    
    builder.Services.AddScoped<IRegisterUseCase, RegisterUseCase>();

    builder.Services.AddScoped<IUserGrpcClient, UserGrpcSafeClient>();
    builder.Services.AddScoped<INotificationGrpcClient, NotificationGrpcSafeClient>();

    builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("Google"));
    builder.Services.Configure<JwtTokenOptions>(builder.Configuration.GetSection("Jwt"));

    builder.RegisterGrpcClient<NotificationGrpcClient>("NotificationClient");
    builder.RegisterGrpcClient<UserGrpcClient>("UserClient"); 
}

void ConfigureApp(WebApplication app)
{
    app.ApplyPostgresMigrations();
    //using var scope = app.Services.CreateScope();
    //var db = scope.ServiceProvider.GetRequiredService<PgContext>();
    //db.Database.Migrate();
    //using var scope = app.Services.CreateScope();
    //var db = scope.ServiceProvider.GetRequiredService<BasePgContext>();
    //db.Database.Migrate();


    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseCors("AllowAll");
    //app.UseCors("AllowFrontend");
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

#endregion Setup Helpers

