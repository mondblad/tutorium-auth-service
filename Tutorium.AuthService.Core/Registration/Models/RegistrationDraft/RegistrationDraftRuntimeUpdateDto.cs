namespace Tutorium.AuthService.Core.Registration.Models.RegistrationDraft
{
    public class RegistrationDraftRuntimeUpdateDto : IRegistrationDraftDto
    {
        public string? Email { get; set; }

        public string? Login { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
    }
}
