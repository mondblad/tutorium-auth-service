using Tutorium.Shared.Utils.BaseModel;

namespace Tutorium.AuthService.Core.Registration.Models.RegistrationDraft
{
    public enum RegistrationDraftStateStatusEnum
    {
        UserIsExsist = 0,
        WaitSendCode = 1,
        CodeWasSend = 2
    }

    public class RegistrationDraftState : BaseModel
    {
        public string Email { get; init; }
        public string Login { get; init; }
        public string FirstName { get; init; }
        public string SecondName { get; init; }

        public RegistrationDraftStateStatusEnum Status { get; set; }

        public string IP { get; init; }
        public string UserAgent { get; init; }
        public DateTime StartRegistrationAt { get; init; }

        protected RegistrationDraftState() { }

        public RegistrationDraftState(RegistrationDraftRuntime draftRuntime)
        {
            Email = draftRuntime.Email ?? throw new ArgumentNullException(nameof(draftRuntime.Email));
            Login = draftRuntime.Login ?? throw new ArgumentNullException(nameof(draftRuntime.Login));
            FirstName = draftRuntime.FirstName ?? throw new ArgumentNullException(nameof(draftRuntime.FirstName));
            SecondName = draftRuntime.SecondName ?? throw new ArgumentNullException(nameof(draftRuntime.SecondName));

            IP = draftRuntime.IP ?? throw new ArgumentNullException(nameof(draftRuntime.IP));
            UserAgent = draftRuntime.UserAgent ?? throw new ArgumentNullException(nameof(draftRuntime.UserAgent));
            StartRegistrationAt = draftRuntime.StartRegistrationAt;
        }
    }
}
