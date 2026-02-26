using Tutorium.AuthService.Application.Identity.Abstractions.Security;
using Tutorium.AuthService.Core.Identity.ValueObjects;

namespace Tutorium.AuthService.Infrastructure.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public PasswordHash Hash(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            return PasswordHash.Create(hash);
        }

        public bool Verify(string password, PasswordHash hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash.Value);
        }
    }
}
