namespace Tutorium.AuthService.Core.Registration.Models.RegistrationDraft
{
    public interface IRegistrationDraftDto
    {
        public string Email { get; }
        public string Login { get; }
        public string FirstName { get; }
        public string SecondName { get; }
    }
}
