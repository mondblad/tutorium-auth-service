using StackExchange.Redis;

namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider
{
    internal interface ISessionDatabase
    {
        IDatabase Db { get; }
    }
}
