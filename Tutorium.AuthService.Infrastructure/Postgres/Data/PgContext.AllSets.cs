using Microsoft.EntityFrameworkCore;
using Tutorium.AuthService.Core.Identity;
using Tutorium.AuthService.Core.Identity.Entities;

namespace Tutorium.AuthService.Infrastructure.Postgres
{
    public partial class PgContext
    {
        public DbSet<UserAuthentication> UserAuthentications { get; set; }
        public DbSet<EmailAuthentication> EmailAuthentications { get; set; }
    }
}
