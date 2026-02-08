using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;
using Tutorium.Shared.Utils.BaseModel;

namespace Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt
{
    public enum RegistrationAttemptStatus
    {
        Create = 0,
        UserIsExist = 1,
        Confirm = 2,
        SecondCode = 3,
        Close = 4,
        CodeWasSend = 5,
    }

    public class RegistrationAttemptState : BaseModel
    {
        protected RegistrationAttemptState() { }

        public RegistrationAttemptState(int registrationDraftStateId)
        {
            RegistrationDraftStateId = registrationDraftStateId;
            Status = RegistrationAttemptStatus.Create;
            CreatedAt = DateTime.UtcNow;
        }

        public int RegistrationDraftStateId { get; private set; }
        public RegistrationDraftState RegistrationDraftState { get; private set; }

        public RegistrationAttemptStatus Status { get; set; }
        public DateTime CreatedAt { get; private set; }
    }
}
