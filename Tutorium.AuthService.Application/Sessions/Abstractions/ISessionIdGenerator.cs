namespace Tutorium.AuthService.Application.Sessions.Abstractions
{
    public interface ISessionIdGenerator
    {
        string GenerateSessionId();
    }
}
