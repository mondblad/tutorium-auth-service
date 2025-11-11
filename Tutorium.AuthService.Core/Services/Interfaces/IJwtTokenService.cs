namespace Tutorium.AuthService.Core.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email);
        string BuildRedirectUrl(int userId, string email);
    }
}
