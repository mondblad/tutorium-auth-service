using Tutorium.Shared.Utils.Redis.Base;
using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;
using StackExchange.Redis;
using Tutorium.AuthService.Core.Registration.Abstractions;

namespace Tutorium.AuthService.Infrastructure.Redis.Repositories
{
    internal class RedisRegistrationDraftRuntimeRepository : BaseRedisRepository<RegistrationDraftRuntime>, IRegistrationDraftRuntimeRepository
    {
        public RedisRegistrationDraftRuntimeRepository(IConnectionMultiplexer redis) : base(redis) { }
    }
}
