using Tutorium.AuthService.Application.Identity.Runtime;
using Tutorium.AuthService.Application.Identity.ValueObjects;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Application.Identity.Abstractions.UseCases
{
    public interface IRegistrationUseCase
    {
        Task<Guid> InitiateRegistrationAsync(Email email, string password);
        Task<string?> ConfirmRegistrationAsync(Guid Token, VerificationCode code);
    }
}
