namespace Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt
{
    public class RegistrationAttemptDto
    {
        public Ulid Token { get; init; }
        public string? Email { get; set; }
    }

    public class RegistrationAttemptUpdateDto
    {
        public string? Email { get; set; }
    }

    public class StartRegDto
    {
        //public Ulid Token { get; init; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        //public DateTime CreatedAtUtc { get; init; } = null!;
        //public int AttemptsCount { get; set; } = null!;



    }

    public enum State
    {
        SendCode = 0,
        Confirm = 1,
        Close = 2
    }

    public class RegistrationAttempt
    {
        public Ulid DraftToken { get; init; }


        public string Email { get; init; } = null!;
        public string PasswordHash { get; init; } = null!;
        public string ConfirmationCode { get; set; } = null!;
    }

    /*public class RegistrationAttemptState
    {
        public Ulid Token { get; init; }
        public string Email { get; init; } = null!;
        public string PasswordHash { get; init; } = null!;
        public string ConfirmationCode { get; set; } = null!;

        public DateTime StartRegistrationAt { get; init; }
        
        public DateTime GeneratedCodeAt { get; set; }
        public int AttemptsCount { get; set; }

        public RegistrationAttemptState(Ulid token, StartRegDto startRegDto, RegistrationAttempt attempt)
        {
            Token = token;
            Email = startRegDto.Email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(startRegDto.Password);

            StartRegistrationAt = attempt.StartRegistrationAt;
        }

        public void GenerateConfirmationCode()
        {
            GeneratedCodeAt = DateTime.Now;
            ConfirmationCode = Random.Shared.Next(100_000, 999_999).ToString();
        }
    }*/

    /*public class RegistrationAttempt
    {
        public Ulid Token { get; init; } 
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? ConfirmationCode { get; set; }
        public DateTime StartRegistrationAt { get; init; }
        public int AttemptsCount { get; set; }

        public RegistrationAttempt()
        {
            Token = Ulid.NewUlid();
            AttemptsCount = 0;
            StartRegistrationAt = DateTime.UtcNow;
        }

        public void Update(RegistrationAttemptUpdateDto updateDto)
        {
            Email = updateDto.Email;
        }

        public void Update(StartRegDto startRegDto)
        {
            Email = startRegDto.Email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(startRegDto.Password);
            ConfirmationCode = Random.Shared.Next(100_000, 999_999).ToString();
        }

        public RegistrationAttemptDto GetRegistrationAttemptDto()
        {
            return new RegistrationAttemptDto()
            {
                Token = this.Token,
                Email = this.Email,
            };
        }
    }*/
}
