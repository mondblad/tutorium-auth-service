using StackExchange.Redis;

namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider
{
    internal class IdentityDatabase : IIdentityDatabase
    {
        public IDatabase Db { get; }
        public IdentityDatabase(IDatabase db) => Db = db;
    }
}
