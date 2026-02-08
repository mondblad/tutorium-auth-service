using Tutorium.Shared.Utils.Redis.Abstractions;
using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegistrationDraftRuntimeRepository : IRuntimeRepository<RegistrationDraftRuntime>
    {
    }
}
