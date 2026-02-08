using Tutorium.AuthService.Core.Abstractions;
using Tutorium.AuthService.Core.Exceptions;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt;
using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;

namespace Tutorium.AuthService.Core.Registration.UseCase
{
    public class RegisterUseCase : IRegisterUseCase
    {
        private readonly IUserGrpcClient _userGrpcClient;
        private readonly INotificationGrpcClient _notificationGrpcClient;

        private readonly IRegistrationAttemptRuntimeRepository _attemptRuntimeRepository;
        private readonly IRegistrationAttemptStateRepository _attemptStateRepository;
        private readonly IRegistrationDraftRuntimeRepository _draftRuntimeRepository;
        private readonly IRegistrationDraftStateRepository _draftStateRepository;

        public RegisterUseCase(
            IUserGrpcClient userGrpcClient,
            INotificationGrpcClient notificationGrpcClient,
            IRegistrationAttemptRuntimeRepository attemptRuntimeRepository,
            IRegistrationAttemptStateRepository attemptStateRepository,
            IRegistrationDraftRuntimeRepository draftRuntimeRepository,
            IRegistrationDraftStateRepository draftStateRepository)
        {
            _userGrpcClient = userGrpcClient;

            _notificationGrpcClient = notificationGrpcClient;
            _attemptRuntimeRepository = attemptRuntimeRepository;
            _attemptStateRepository = attemptStateRepository;
            _draftRuntimeRepository = draftRuntimeRepository;
            _draftStateRepository = draftStateRepository;
        }

        public async Task<Guid> CreateRegistrationDraft(RegistrationDraftRuntimeCreateDto createDto)
        {
            var attempt = new RegistrationDraftRuntime(createDto);

            await _draftRuntimeRepository.Add(attempt);

            return attempt.Token;
        }

        public async Task UpdateRegistrationDraft(Guid token, RegistrationDraftRuntimeUpdateDto updateDto)
        {
            var draft = await _draftRuntimeRepository.GetByTokenAsync(token);

            if (draft is null)
                throw new RegistrationAttemptNotFoundException();

            draft.Update(updateDto);

            await _draftRuntimeRepository.Update(draft);
        }

        public async Task<RegistrationDraftRuntimeDto> GetRegistrationDraft(Guid token)
        {
            var draft = await _draftRuntimeRepository.GetByTokenAsync(token);

            if (draft is null)
                throw new RegistrationAttemptNotFoundException();

            return draft.GetRegistrationDraftRuntimeDto();
        }

        public async Task<Guid> SendConfirmationCode(Guid token, RegistrationDraftRuntimeSubmitDto submitDto)
        {
            var draft = await _draftRuntimeRepository.GetByTokenAsync(token);

            if (draft is null)
                throw new RegistrationAttemptNotFoundException();

            draft.Update(submitDto);
            await _draftRuntimeRepository.Update(draft);

            RegistrationDraftState draftState = new RegistrationDraftState(draft);
            _draftStateRepository.Add(draftState);
            await _draftStateRepository.SaveChangesAsync();

            RegistrationAttemptState attemptState = new RegistrationAttemptState(draftState.Id);
            _attemptStateRepository.Add(attemptState);
            await _attemptStateRepository.SaveChangesAsync();

            try
            {
                var isUserExists = await _userGrpcClient.IsUserExistsAsync(draft.Email!);
                if (isUserExists)
                {
                    attemptState.Status = RegistrationAttemptStatus.UserIsExist;
                    throw new UserAlreadyExistsException(draft.Email!);
                }

                var attemptRuntime = new RegistrationAttemptRuntime(attemptState.Id);
                await _attemptRuntimeRepository.Add(attemptRuntime);

                await _notificationGrpcClient.SendEmailVerificationCodeAsync(draftState.Email, attemptRuntime.ConfirmationCode);

                attemptState.Status = RegistrationAttemptStatus.CodeWasSend;

                return attemptRuntime.Token;
            }
            finally
            {
                await _attemptStateRepository.SaveChangesAsync();
            }
        }

        public async Task ConfirmRegistration(Guid token, string code)
        {
            var attempt = await _attemptRuntimeRepository.GetByTokenAsync(token);

            if (attempt is null)
                throw new RegistrationAttemptNotFoundException();
            else if (attempt.ConfirmationCode != code)
                throw new InvalidConfirmationCodeException();

            _attemptStateRepository.Query();

            await _userGrpcClient.CreateUserAsync(attempt.Email, attempt.PasswordHash, attempt.StartRegistrationAt);
        }
    }
}
