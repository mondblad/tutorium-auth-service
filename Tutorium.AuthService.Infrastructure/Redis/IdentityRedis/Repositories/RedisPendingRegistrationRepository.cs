using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Application.Identity.Runtime;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider;
using Tutorium.Shared.Utils.Redis.Base;

namespace Tutorium.AuthService.Infrastructure.Redis.IdentityRedis.Repositories
{
    internal class RedisPendingRegistrationRepository : BaseRedisRepository<PendingRegistration>, IPendingRegistrationRepository
    {
        public RedisPendingRegistrationRepository(IIdentityDatabase identityDatabase) : base(identityDatabase.Db) { }
    }
}
