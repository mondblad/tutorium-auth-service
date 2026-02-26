using Microsoft.EntityFrameworkCore;
using Tutorium.AuthService.Core.Identity;
using Tutorium.AuthService.Core.Identity.Entities;
using Tutorium.Shared.Utils.BaseModel;
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

            //ModelBuilderExtensions.ApplySeparateTableAttribute(modelBuilder);

            var entityTypesWithId = modelBuilder.Model.GetEntityTypes().Where(t => typeof(IBaseModel).IsAssignableFrom(t.ClrType));

            foreach (var entityType in entityTypesWithId)
            {
                var clrType = entityType.ClrType;

                var baseType = entityType.BaseType?.ClrType;

                if (baseType != null && typeof(IBaseModel).IsAssignableFrom(baseType))
                    continue;

                var idProperty = clrType.GetProperty(nameof(IBaseModel.Id));
                if (idProperty != null)
                    modelBuilder.Entity(clrType).HasKey(nameof(IBaseModel.Id));
            }

            modelBuilder.Entity<EmailAuthentication>(builder =>
            {
                builder.OwnsOne(
                    x => x.Email,
                    b => b.Property(p => p.Value).HasColumnName(nameof(EmailAuthentication.Email)).IsRequired()
                );
                builder.OwnsOne(
                    x => x.PasswordHash, 
                    b => b.Property(p => p.Value).HasColumnName(nameof(EmailAuthentication.PasswordHash)).IsRequired()
                );
            });
        }
    }
}
