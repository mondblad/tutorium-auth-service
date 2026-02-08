using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Infrastructure.Redis.Repositories;

namespace Tutorium.AuthService.Infrastructure.Redis
{
    public static class RedisModule
    {
        public static IServiceCollection AddRedisModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionRedisString = configuration.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(connectionRedisString))
                throw new InvalidOperationException("Redis connection string is not configured");

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(connectionRedisString));

            services.AddScoped<IRegistrationDraftRuntimeRepository, RedisRegistrationDraftRuntimeRepository>();
            services.AddScoped<IRegistrationAttemptRuntimeRepository, RedisRegistrationAttemptRuntimeRepository>();

            return services;
        }
    }
}
