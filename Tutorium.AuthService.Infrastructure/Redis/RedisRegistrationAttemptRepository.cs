using StackExchange.Redis;
using System.Text.Json;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models;

namespace Tutorium.AuthService.Infrastructure.Redis
{
    public class RedisRegistrationAttemptRepository : IRegistrationAttemptRepository
    {
        private readonly IDatabase _db;

        public RedisRegistrationAttemptRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task Add(RegistrationAttempt attempt) => await SaveOrUpdateAsync(attempt);

        public async Task Update(RegistrationAttempt attempt) => await SaveOrUpdateAsync(attempt);

        public async Task<RegistrationAttempt?> GetByTokenAsync(Ulid token)
        {
            var value = await _db.StringGetAsync(token.ToString());
            if (!value.HasValue)
                return null;

            return JsonSerializer.Deserialize<RegistrationAttempt>(value!);
        }

        private async Task SaveOrUpdateAsync(RegistrationAttempt attempt)
        {
            var json = JsonSerializer.Serialize(attempt);

            await _db.StringSetAsync(attempt.Token.ToString(), json, new TimeSpan(0, 10, 0));
        }
    }
}
