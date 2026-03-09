using Tutorium.AuthService.Application.Identity.Abstractions.Security;
using Tutorium.Shared.Utils.Exceptions;

namespace Tutorium.AuthService.Infrastructure.Security
{
    public class PasswordValidatorException : InfrastructureException
    {
        public PasswordValidatorException(string errorCode, string message) : base(errorCode, message) { }
    }

    public class PasswordValidator : IPasswordValidator
    {
        public void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new PasswordValidatorException("PASSWORD_REQUIRED", "Password is required");

            if (password.Length < 8)
                throw new PasswordValidatorException("PASSWORD_TOO_SHORT", "Password must be at least 8 characters");

            if (!password.Any(char.IsUpper))
                throw new PasswordValidatorException("PASSWORD_NO_UPPERCASE", "Password must contain uppercase letter");
        }
    }
}
