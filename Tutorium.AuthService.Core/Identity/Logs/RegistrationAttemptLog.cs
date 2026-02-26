using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Core.Identity.Logs
{
    public class RegistrationAttemptLog
    {
        public Email Email { get; init; }

        public RegistrationAttemptLog(Email email)
        {
            Email = email;
        }
    }
}
