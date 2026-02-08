using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;
using Tutorium.Shared.Utils.EntityFramework.Base;

namespace Tutorium.AuthService.Infrastructure.Postgres.Repositories
{
    internal class RegistrationDraftStateRepository : BasePostgresCRUDRepository<RegistrationDraftState, PgContext>, IRegistrationDraftStateRepository
    {
        public RegistrationDraftStateRepository(PgContext context) : base(context) { }
    }
}
