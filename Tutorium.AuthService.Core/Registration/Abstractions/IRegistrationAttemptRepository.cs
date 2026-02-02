using Tutorium.AuthService.Core.Registration.Models;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegistrationAttemptRepository
    {
        //Task<RegistrationAttempt?> GetByTokenAsync(string token);
        //Task SaveAsync(RegistrationAttempt attempt, TimeSpan ttl);
        //Task RemoveAsync(string token);
        Task Add(RegistrationAttempt attempt);
        Task Update(RegistrationAttempt attempt);
        Task<RegistrationAttempt?> GetByTokenAsync(Ulid token);
    }
}
