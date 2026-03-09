using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorium.AuthService.Application.Sessions.Abstractions;
using Tutorium.AuthService.Application.Sessions.Services;

namespace Tutorium.AuthService.Application.Sessions
{
    internal static class SessionsAppModule
    {
        public static IServiceCollection AddSessionsAppModule(this IServiceCollection services)
        {
            services.AddScoped<ISessionManager, SessionManager>();

            return services;
        }
    }
}
