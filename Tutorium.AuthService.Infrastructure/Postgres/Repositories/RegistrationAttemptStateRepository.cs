using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt;
using Tutorium.Shared.Utils.EntityFramework.Base;

namespace Tutorium.AuthService.Infrastructure.Postgres.Repositories
{
    internal class RegistrationAttemptStateRepository : BasePostgresCRUDRepository<RegistrationAttemptState, PgContext>, IRegistrationAttemptStateRepository
    {
        public RegistrationAttemptStateRepository(PgContext context) : base(context) { }
    }
}
