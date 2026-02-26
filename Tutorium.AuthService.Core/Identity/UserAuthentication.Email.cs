using Tutorium.AuthService.Core.Identity.Abstractions;
using Tutorium.AuthService.Core.Identity.ValueObjects;
using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.AuthService.Core.Identity.Entities;

namespace Tutorium.AuthService.Core.Identity
{
    public partial class UserAuthentication
    {
        public static UserAuthentication CreateByEmailAuthentication(int userId, Email email, PasswordHash passwordHash, IEmailUniquenessChecker emailUniquenessChecker)
        {
            var emailAuthentication = EmailAuthentication.Create(email, passwordHash);

            return new UserAuthentication(userId, emailAuthentication);
        }

        public void UpdateEmailAuthentication(Email email, PasswordHash passwordHash, IEmailUniquenessChecker emailUniquenessChecker)
        {
            if (!emailUniquenessChecker.IsUnique(email))
                throw new InvalidOperationException("Email already in use");

            var emailAuthentication = EmailAuthentication.Create(email, passwordHash);

            ByEmail = emailAuthentication;
        }
    }
}
