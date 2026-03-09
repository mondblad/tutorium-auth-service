using StackExchange.Redis;
using Tutorium.AuthService.Core.Sessions.Models;

namespace Tutorium.AuthService.Application.Sessions.Abstractions
{
    public interface IUserSessionRepository
    {
        Task<UserSessions?> GetByIdAsync(int userId);
        Task SetAsync(UserSessions userSession, ITransaction? transaction = null);
        Task DeleteByIdAsync(int userSessionId, ITransaction? transaction = null);
    }
}
