using Tutorium.AuthService.Core.Abstractions;
using Tutorium.Shared.Utils.Redis.Abstractions;

namespace Tutorium.AuthService.Core.Registration.Models.RegistrationDraft
{
    public class RegistrationDraftRuntime : IWithGuidToken
    {
        public Guid Token { get; init; }
        public string? Email { get; set; }

        public string? Login { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }

        public string IP { get; init; }
        public string UserAgent { get; init; }
        public DateTime StartRegistrationAt { get; init; }

        public RegistrationDraftRuntime() { }

        public RegistrationDraftRuntime(RegistrationDraftRuntimeCreateDto dto)
        {
            Token = Guid.NewGuid();

            IP = dto.IP;
            UserAgent = dto.UserAgent;

            StartRegistrationAt = DateTime.UtcNow;
        }

        public void Update(IRegistrationDraftDto updateDto)
        {
            Email = updateDto.Email;

            Login = updateDto.Login;
            FirstName = updateDto.FirstName;
            SecondName = updateDto.SecondName;
        }

        public RegistrationDraftRuntimeDto GetRegistrationDraftRuntimeDto()
        {
            return new RegistrationDraftRuntimeDto()
            {
                Email = this.Email,
                Login = this.Login,
                FirstName = this.FirstName,
                SecondName = this.SecondName,
            };
        }
    }
}
