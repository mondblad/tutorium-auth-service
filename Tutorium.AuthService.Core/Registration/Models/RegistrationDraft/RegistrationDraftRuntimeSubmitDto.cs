namespace Tutorium.AuthService.Core.Registration.Models.RegistrationDraft
{
    public class RegistrationDraftRuntimeSubmitDto : IRegistrationDraftDto
    {
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
    }
}
