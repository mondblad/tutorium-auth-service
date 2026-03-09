namespace Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Const
{
    internal static class SessionConstants
    {
        public const string RedisConnectionStringName = "RedisSession";

        public const string SessionPrefix = "sess:";
        public const string UserSessionsPrefix = "userSessions:";
        public static readonly TimeSpan Ttl = TimeSpan.FromDays(7);
    }
}
