namespace Tutorium.AuthService.Application.Identity.Abstractions
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId);
    }
}
