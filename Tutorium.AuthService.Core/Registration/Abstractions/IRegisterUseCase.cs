using Tutorium.AuthService.Core.Registration.Models;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegisterUseCase
    {
        Task<Ulid> StartRegistration();
        Task UpdateRegistrationAttempt(RegistrationAttemptDto attempt);
        Task<RegistrationAttemptDto> GetRegistrationAttempt(Ulid token);

        Task<Ulid> StartRegistration(string email, string password);
        Task ConfirmRegistration(Ulid token, string code);
    }
}
