using StackExchange.Redis;

namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider
{
    internal class SessionDatabase : ISessionDatabase
    {
        public IDatabase Db { get; }
        public SessionDatabase(IDatabase db) => Db = db;
    }
}
