using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorium.AuthService.Application.Sessions.Abstractions;
using Tutorium.AuthService.Infrastructure.Postgres;
using Tutorium.AuthService.Infrastructure.Redis;
using Tutorium.AuthService.Infrastructure.Security;
using Tutorium.AuthService.Application.Identity.Abstractions.Security;

namespace Tutorium.AuthService.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IPasswordValidator, PasswordValidator>();
            services.AddScoped<ISessionIdGenerator, SessionIdGenerator>();

            return services
                .AddRedisModule(configuration)
                .AddPostgresModule(configuration);
        }
    }
}
