namespace Tutorium.AuthService.Core.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email);
    }
}
