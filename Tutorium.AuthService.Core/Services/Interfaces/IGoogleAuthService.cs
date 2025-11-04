using Tutorium.AuthService.Core.Models.Google;

namespace Tutorium.AuthService.Core.Services.Interfaces
{
    public interface IGoogleAuthService
    {
        string BuildAuthUrl(string redirectUri);
        Task<GoogleTokenResponse> ExchangeCodeAsync(string code, string redirectUri);
        (string? Email, string? Name) ParseIdToken(string idToken);
    }
}
