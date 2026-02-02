namespace Tutorium.AuthService.Core.Registration.Models
{
    public class RegistrationAttemptDto
    {
        public Ulid Token { get; init; }
        public string? Email { get; set; }
    }

    public class RegistrationAttemptUpdateDto : RegistrationAttemptDto
    {
        public string? Password { get; set; }
    }

    public class RegistrationAttempt
    {
        public Ulid Token { get; init; } 
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? ConfirmationCode { get; set; }
        public DateTime CreatedAtUtc { get; init; }
        public int AttemptsCount { get; set; }

        public RegistrationAttempt()
        {
            Token = Ulid.NewUlid();
            AttemptsCount = 0;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Update(RegistrationAttemptUpdateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Email))
                Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        public RegistrationAttemptDto GetRegistrationAttemptDto()
        {
            return new RegistrationAttemptDto()
            {
                Token = this.Token,
                Email = this.Email,
            };
        }

        public void GenerateConfirmationCode()
        {
            ConfirmationCode = Random.Shared.Next(100_000, 999_999).ToString();
        }
    }
}
