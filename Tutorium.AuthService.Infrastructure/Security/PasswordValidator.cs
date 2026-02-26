using Tutorium.AuthService.Application.Identity.Abstractions.Security;

namespace Tutorium.AuthService.Infrastructure.Security
{
    public class PasswordValidator : IPasswordValidator
    {
        public void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters");

            if (!password.Any(char.IsUpper))
                throw new ArgumentException("Password must contain uppercase letter");
        }
    }
}
