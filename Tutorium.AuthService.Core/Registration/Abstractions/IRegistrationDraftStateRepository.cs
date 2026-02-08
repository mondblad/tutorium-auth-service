using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;
using Tutorium.Shared.Utils.EntityFramework.Abstractions;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegistrationDraftStateRepository : IPostgresCRUDRepository<RegistrationDraftState>
    {
    }
}
