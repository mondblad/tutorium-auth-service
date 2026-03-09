using StackExchange.Redis;
using Tutorium.AuthService.Core.Sessions.Models;

namespace Tutorium.AuthService.Application.Sessions.Abstractions
{
    public interface ISessionRepository
    {
        Task<Session?> GetByIdAsync(string sessionId);
        Task SetAsync(Session session, ITransaction? transaction = null);
        Task DeleteByIdAsync(string sessionId, ITransaction? transaction = null);
        Task<List<Session>> GetByIdsAsync(IEnumerable<string> sessionIds);
    }
}
