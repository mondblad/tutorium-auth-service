using StackExchange.Redis;

namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider
{
    internal interface IIdentityDatabase
    {
        IDatabase Db { get; }
    }
}
