using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Tutorium.AuthService.Application.Sessions.Abstractions;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Const;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Repositories;

namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis
{
    internal static class SessionRedisModule
    {
        public static IServiceCollection AddSessionRedisModule(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionStringName = SessionConstants.RedisConnectionStringName;
            var connectionRedisString = configuration.GetConnectionString(redisConnectionStringName);
            
            if (string.IsNullOrWhiteSpace(connectionRedisString))
                throw new InvalidOperationException($"{redisConnectionStringName} connection string is not configured");

            services.AddSingleton<ISessionDatabase>(sp =>
            {
                var multiplexer = ConnectionMultiplexer.Connect(connectionRedisString);
                return new SessionDatabase(multiplexer.GetDatabase());
            });

            services.AddScoped<IUserSessionRepository, RedisUserSessionRepository>();
            services.AddScoped<ISessionRepository, RedisSessionRepository>();
            services.AddScoped<ISessionUnitOfWork, SessionUnitOfWork>();

            return services;
        }
    }
}
