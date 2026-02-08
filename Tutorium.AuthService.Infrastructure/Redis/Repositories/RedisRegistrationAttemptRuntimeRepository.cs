using StackExchange.Redis;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt;
using Tutorium.Shared.Utils.Redis.Base;

namespace Tutorium.AuthService.Infrastructure.Redis.Repositories
{
    internal class RedisRegistrationAttemptRuntimeRepository : BaseRedisRepository<RegistrationAttemptRuntime>, IRegistrationAttemptRuntimeRepository
    {
        public RedisRegistrationAttemptRuntimeRepository(IConnectionMultiplexer redis) : base(redis) { }
    }
}
