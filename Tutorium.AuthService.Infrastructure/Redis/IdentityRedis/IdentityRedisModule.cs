using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Infrastructure.Redis.IdentityRedis.Const;
using Tutorium.AuthService.Infrastructure.Redis.IdentityRedis.Repositories;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider;

namespace Tutorium.AuthService.Infrastructure.Redis.IdentityRedis
{
    internal static class IdentityRedisModule
    {
        public static IServiceCollection AddIdentityRedisModule(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionStringName = IdentityConstants.RedisConnectionStringName;
            var connectionRedisString = configuration.GetConnectionString(redisConnectionStringName);

            if (string.IsNullOrWhiteSpace(connectionRedisString))
                throw new InvalidOperationException($"{redisConnectionStringName} connection string is not configured");

            services.AddSingleton<IIdentityDatabase>(sp =>
            {
                var multiplexer = ConnectionMultiplexer.Connect(connectionRedisString);
                return new IdentityDatabase(multiplexer.GetDatabase());
            });

            services.AddScoped<IPendingRegistrationRepository, RedisPendingRegistrationRepository>();

            return services;
        }
    }
}
