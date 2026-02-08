using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegisterUseCase
    {
        Task<Guid> CreateRegistrationDraft(RegistrationDraftRuntimeCreateDto createDto);
        Task UpdateRegistrationDraft(Guid token, RegistrationDraftRuntimeUpdateDto updateDto);
        Task<RegistrationDraftRuntimeDto> GetRegistrationDraft(Guid token);

        Task<Guid> SendConfirmationCode(Guid token, RegistrationDraftRuntimeSubmitDto submitDto);
        //Task ConfirmRegistration(Ulid token, string code);
    }
}
