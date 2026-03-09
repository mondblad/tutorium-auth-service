namespace Tutorium.AuthService.Core.Sessions.Models
{
    public class Session
    {
        public string SessionId { get; protected set; }
        public int UserId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        //public string? UserAgent { get; set; }
        //public string? IpAddress { get; set; }

        public Session(string sessionId, int userId)
        {
            SessionId = sessionId;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
