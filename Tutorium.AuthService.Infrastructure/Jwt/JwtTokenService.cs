using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tutorium.AuthService.Core.Models.JwtToken;
using Tutorium.AuthService.Core.Services.Interfaces;

namespace Tutorium.AuthService.Infrastructure.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenOptions _jwtTokenOptions;

        public JwtTokenService(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;

            if (_jwtTokenOptions.Secret is null || _jwtTokenOptions.FrontendUrl is null)
                throw new Exception("Missing JWT Options");
        }

        public string GenerateToken(int userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: JwtConst.JWT_ISSUER,
                audience: JwtConst.JWT_AUDIENCE_FRONTEND,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string BuildRedirectUrl(int userId, string email)
            => $"{_jwtTokenOptions.FrontendUrl}/oauth/callback?token={GenerateToken(userId, email)}";
    }
}
