using Tutorium.AuthService.Application.Identity.Runtime;
using Tutorium.Shared.Utils.Redis.Abstractions;

namespace Tutorium.AuthService.Application.Identity.Abstractions
{
    public interface IPendingRegistrationRepository : IRuntimeRepository<PendingRegistration>
    {
    }
}
