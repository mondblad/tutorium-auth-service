using Microsoft.EntityFrameworkCore;
using Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt;
using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;

namespace Tutorium.AuthService.Infrastructure.Postgres
{
    public partial class PgContext
    {
        public DbSet<RegistrationDraftState> RegistrationDraftStates { get; set; }
        public DbSet<RegistrationAttemptState> RegistrationAttemptStates { get; set; }
    }
}
