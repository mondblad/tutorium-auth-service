using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Tutorium.AuthService.Core.Models.JwtToken;
using Tutorium.AuthService.Core.Services.Interfaces;

namespace Tutorium.AuthService.Core.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly string _secretKey;

        public JwtTokenService(JwtTokenOptions jwtTokenOptions)
        {
            _secretKey = jwtTokenOptions.Secret ?? throw new Exception("Missing JWT Secret");
        }

        public string GenerateToken(int userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: "tutorium-auth",
                audience: "tutorium-frontend",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
