using Tutorium.AuthService.Core.Sessions.Models;

namespace Tutorium.AuthService.Application.Sessions.Abstractions
{
    public interface ISessionManager
    {
        Task<Session?> GetSessionAsync(string sessionId);
        Task<List<Session>> GetUserSessionsAsync(int userId);
        Task<Session> CreateSessionAsync(int userId);
        Task DeleteSessionAsync(string sessionId);
        Task<Session> ResetUserSessionsAsync(int userId);
    }
}
