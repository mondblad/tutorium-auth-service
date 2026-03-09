using Tutorium.AuthService.Application.Identity.ValueObjects;
using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.AuthService.Core.Sessions.Models;

namespace Tutorium.AuthService.Application.Identity.Abstractions.UseCases
{
    public interface IRegistrationUseCase
    {
        Task<Guid> InitiateRegistrationAsync(Email email, string password);
        Task<Session> ConfirmRegistrationAsync(Guid Token, VerificationCode code);
        Task<Email> GetEmailAsync(Guid token);
    }
}
