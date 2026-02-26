using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tutorium.AuthService.Core.Models.JwtToken;
using Tutorium.AuthService.Application.Identity.Abstractions;

namespace Tutorium.AuthService.Infrastructure.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenOptions _jwtTokenOptions;

        public JwtTokenService(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;

            if (jwtTokenOptions is null
                || string.IsNullOrEmpty(_jwtTokenOptions.Secret) 
                || string.IsNullOrEmpty(_jwtTokenOptions.FrontendUrl)
                || string.IsNullOrEmpty(_jwtTokenOptions.Issuer)
                || string.IsNullOrEmpty(_jwtTokenOptions.Audience))
                throw new Exception("Missing JWT Options");
        }

        public string GenerateToken(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtTokenOptions.Issuer,
                audience: _jwtTokenOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public string BuildRedirectUrl(int userId, string email)
        //    => $"{_jwtTokenOptions.FrontendUrl}/oauth/callback?token={GenerateToken(userId, email)}";
    }
}
