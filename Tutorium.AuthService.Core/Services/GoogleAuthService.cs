using System.IdentityModel.Tokens.Jwt;
using System.Web;
using Tutorium.AuthService.Core.Models.Google;
using Tutorium.AuthService.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Tutorium.AuthService.Core.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleOptions _google;
        private readonly HttpClient _httpClient;

        public GoogleAuthService(IOptions<GoogleOptions> googleOptions, HttpClient httpClient)
        {
            _google = googleOptions.Value;
            _httpClient = httpClient;
        }

        public string BuildAuthUrl(string redirectUri)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = _google.ClientId;
            query["redirect_uri"] = redirectUri;
            query["response_type"] = "code";
            query["scope"] = "openid email profile";
            query["prompt"] = "select_account";

            var uriBuilder = new UriBuilder("https://accounts.google.com/o/oauth2/auth")
            {
                Query = query.ToString()
            };

            return uriBuilder.ToString();
        }

        public async Task<GoogleTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var tokenRequest = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _google.ClientId,
                ["client_secret"] = _google.ClientSecret,
                ["redirect_uri"] = redirectUri,
                ["grant_type"] = "authorization_code"
            };

            var response = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequest));
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Google token exchange failed: {responseText}");

            return System.Text.Json.JsonSerializer.Deserialize<GoogleTokenResponse>(responseText)
                   ?? throw new Exception("Invalid token response from Google");
        }

        public (string? Email, string? Name) ParseIdToken(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(idToken);

            var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

            return (email, name);
        }
    }
}
