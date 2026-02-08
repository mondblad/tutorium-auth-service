using Microsoft.EntityFrameworkCore;
using Tutorium.Shared.Utils.EntityFramework.Base;
using Tutorium.Shared.Utils.EntityFramework.Extensions;

namespace Tutorium.AuthService.Infrastructure.Postgres
{
    public partial class PgContext : BasePgContext
    {
        public PgContext(DbContextOptions<PgContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelBuilderExtensions.ApplySeparateTableAttribute(modelBuilder);
        }
    }
}
