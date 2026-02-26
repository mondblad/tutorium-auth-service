using Tutorium.AuthService.Core.Identity.ValueObjects;

namespace Tutorium.AuthService.Application.Identity.Abstractions.Security
{
    public interface IPasswordHasher
    {
        PasswordHash Hash(string password);
        bool Verify(string password, PasswordHash hash);
    }
}
