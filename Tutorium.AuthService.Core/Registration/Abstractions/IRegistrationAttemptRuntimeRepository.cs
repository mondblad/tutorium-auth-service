using Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt;
using Tutorium.Shared.Utils.Redis.Abstractions;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegistrationAttemptRuntimeRepository : IRuntimeRepository<RegistrationAttemptRuntime>
    {
    }
}
