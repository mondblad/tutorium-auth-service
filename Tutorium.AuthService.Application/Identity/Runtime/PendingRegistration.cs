using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.AuthService.Core.Identity.ValueObjects;
using Tutorium.AuthService.Application.Identity.ValueObjects;
using Tutorium.Shared.Utils.Redis.Abstractions;
using System.Text.Json.Serialization;

namespace Tutorium.AuthService.Application.Identity.Runtime
{
    public class PendingRegistration : IWithGuidToken
    {
        public Guid Token { get; init; }
        public Email Email { get; init; }
        public PasswordHash PasswordHash { get; init; }
        public VerificationCode VerificationCode { get; init; }

        [JsonConstructor]
        private PendingRegistration(Guid token, Email email, PasswordHash passwordHash, VerificationCode verificationCode) 
            => (Token, Email, PasswordHash, VerificationCode) = (token, email, passwordHash, verificationCode);

        public PendingRegistration(Email email, PasswordHash passwordHash, VerificationCode verificationCode) 
            : this(Guid.NewGuid(), email, passwordHash, verificationCode) { }
    }
}
