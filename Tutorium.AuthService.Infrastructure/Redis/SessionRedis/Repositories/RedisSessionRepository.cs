using StackExchange.Redis;
using System.Text.Json;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Const;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider;
using Tutorium.AuthService.Application.Sessions.Abstractions;
using Tutorium.AuthService.Core.Sessions.Models;

namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Repositories
{
    internal class RedisSessionRepository : ISessionRepository
    {
        private readonly IDatabase _db;

        public RedisSessionRepository(ISessionDatabase sessionDatabase)
        {
            _db = sessionDatabase.Db;
        }

        public async Task<Session?> GetByIdAsync(string sessionId)
        {
            var key = $"{SessionConstants.SessionPrefix}{sessionId}";
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<Session>(value!) : null;
        }

        public async Task SetAsync(Session session, ITransaction? transaction = null)
        {
            var key = $"{SessionConstants.SessionPrefix}{session.SessionId}";
            var value = JsonSerializer.Serialize(session);

            if (transaction is not null)
                _ = transaction.StringSetAsync(key, value, SessionConstants.Ttl);
            else
                await _db.StringSetAsync(key, value, SessionConstants.Ttl);
        }

        public async Task DeleteByIdAsync(string sessionId, ITransaction? transaction = null)
        {
            var key = $"{SessionConstants.SessionPrefix}{sessionId}";

            if (transaction is not null)
                await transaction.KeyDeleteAsync(key);
            else
                await _db.KeyDeleteAsync(key);
        }

        public async Task<List<Session>> GetByIdsAsync(IEnumerable<string> sessionIds)
        {
            if (!sessionIds.Any())
                return new List<Session>();

            var keys = sessionIds.Select(id => (RedisKey)$"{SessionConstants.SessionPrefix}{id}").ToArray();
            var values = await _db.StringGetAsync(keys);

            var sessions = new List<Session>();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].HasValue)
                {
                    var session = JsonSerializer.Deserialize<Session>(values[i]!);
                    if (session is not null)
                        sessions.Add(session);
                }
            }

            return sessions;
        }
    }
}
