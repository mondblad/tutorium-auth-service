using StackExchange.Redis;
using Tutorium.AuthService.Application.Identity.Runtime;
using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.Shared.Utils.Redis.Base;

namespace Tutorium.AuthService.Infrastructure.Redis.Repositories
{
    internal class RedisPendingRegistrationRepository : BaseRedisRepository<PendingRegistration>, IPendingRegistrationRepository
    {
        public RedisPendingRegistrationRepository(IConnectionMultiplexer redis) : base(redis) { }
    }
}
