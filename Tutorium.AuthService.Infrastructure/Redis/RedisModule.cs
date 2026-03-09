using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorium.AuthService.Infrastructure.Redis.IdentityRedis;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis;

namespace Tutorium.AuthService.Infrastructure.Redis
{
    internal static class RedisModule
    {
        public static IServiceCollection AddRedisModule(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddIdentityRedisModule(configuration)
                .AddSessionRedisModule(configuration);
        }
    }
}
