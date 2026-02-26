using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tutorium.AuthService.Api.Middleware;
using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Application.Identity.Abstractions.Security;
using Tutorium.AuthService.Application.Identity.Abstractions.UseCases;
using Tutorium.AuthService.Application.Identity.UseCase;
using Tutorium.AuthService.Core.Abstractions;
using Tutorium.AuthService.Core.Models.Google;
using Tutorium.AuthService.Core.Models.JwtToken;
using Tutorium.AuthService.Grpc.Clients;
using Tutorium.AuthService.Infrastructure.Jwt;
using Tutorium.AuthService.Infrastructure.Postgres;
using Tutorium.AuthService.Infrastructure.Redis;
using Tutorium.AuthService.Infrastructure.Security;
using Tutorium.Shared.Utils.Exceptions;
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
    builder.Services.AddControllers(options => {
        options.Filters.Add(new AuthorizeFilter());
    }); 
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("Google"));
    builder.Services.Configure<JwtTokenOptions>(builder.Configuration.GetSection("Jwt"));

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Tutorium API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "¬ведите JWT в формате: Bearer {ваш токен}"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtOptions = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtTokenOptions>();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.Secret))
            };
        });

    builder.Services
        .AddRedisModule(builder.Configuration)
        .AddPostgresModule(builder.Configuration);

    builder.Services.AddSingleton<IExceptionMapper, DefaultExceptionMapper>();
    builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

    builder.Services.AddScoped<IUserGrpcClient, UserGrpcSafeClient>();
    builder.Services.AddScoped<INotificationGrpcClient, NotificationGrpcSafeClient>();

    builder.Services.AddScoped<IRegistrationUseCase, RegistrationUseCase>();

    builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
    builder.Services.AddScoped<IPasswordValidator, PasswordValidator>();

    builder.RegisterGrpcClient<NotificationGrpcClient>("NotificationClient");
    builder.RegisterGrpcClient<UserGrpcClient>("UserClient"); 
}

void ConfigureApp(WebApplication app)
{
    app.ApplyPostgresMigrations();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseCors("AllowFrontend");
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
}

#endregion Setup Helpers

