using StackExchange.Redis;
using Tutorium.AuthService.Infrastructure.Redis.SessionRedis.Provider;
using Tutorium.AuthService.Application.Sessions.Abstractions;

namespace Tutorium.AuthService.Infrastructure.Redis
{
    internal class SessionUnitOfWork : ISessionUnitOfWork
    {
        private readonly IDatabase _database;

        public SessionUnitOfWork(IIdentityDatabase sessionDatabase)
        {
            _database = sessionDatabase.Db;
        }

        public ITransaction BeginTransaction() => _database.CreateTransaction();

        public async Task CommitAsync(ITransaction tran)
        {
            bool success = await tran.ExecuteAsync();
            if (!success)
                throw new Exception("Не удалось выполнить атомарную операцию");
        }
    }
}
