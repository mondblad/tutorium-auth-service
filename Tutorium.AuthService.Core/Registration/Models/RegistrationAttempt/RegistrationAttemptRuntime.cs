using Tutorium.Shared.Utils.Redis.Abstractions;

namespace Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt
{
    public class RegistrationAttemptRuntime : IWithGuidToken
    {
        public Guid Token { get; init; }
        public int AttemptStateId { get; init; }
        public string ConfirmationCode { get; init; } = null!;

        public RegistrationAttemptRuntime(int attemptStateId)
        {
            Token = Guid.NewGuid();
            AttemptStateId = attemptStateId;
            ConfirmationCode = GenerateConfirmationCode();
        }

        private string GenerateConfirmationCode()
        {
            return Random.Shared.Next(100_000, 999_999).ToString();
        }
    }
}
