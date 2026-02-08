using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Infrastructure.Postgres.Repositories;

namespace Tutorium.AuthService.Infrastructure.Postgres
{
    public static class PostgresModule
    {
        public static IServiceCollection AddPostgresModule(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresConnectionString = configuration.GetConnectionString("Postgres");
            if (string.IsNullOrWhiteSpace(postgresConnectionString))
                throw new InvalidOperationException("Postgres connection string is not configured");

            services.AddDbContext<PgContext>(options => options.UseNpgsql(postgresConnectionString));

            services.AddScoped<IRegistrationDraftStateRepository, RegistrationDraftStateRepository>();
            services.AddScoped<IRegistrationAttemptStateRepository, RegistrationAttemptStateRepository>();

            return services;
        }

        public static void ApplyPostgresMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PgContext>();
            db.Database.Migrate();
        }
    }
}
