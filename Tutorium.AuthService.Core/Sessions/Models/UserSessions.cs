namespace Tutorium.AuthService.Core.Sessions.Models
{
    public class UserSessions
    {
        public int UserId { get; protected set; }
        public List<string> SessionIds { get; set; } = new();

        public UserSessions(int userId)
        {
            UserId = userId;
            SessionIds = new();
        }

        public void TryDelete(string sessionId)
        {
            var sessionStr = SessionIds.FirstOrDefault(sessionId);
            if (sessionStr is not null)
                SessionIds.Remove(sessionStr);
        }
    }
}
