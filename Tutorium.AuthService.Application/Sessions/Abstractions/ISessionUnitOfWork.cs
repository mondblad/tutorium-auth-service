using StackExchange.Redis;

namespace Tutorium.AuthService.Application.Sessions.Abstractions
{
    public interface ISessionUnitOfWork
    {
        ITransaction BeginTransaction();
        Task CommitAsync(ITransaction tran);
    }
}
