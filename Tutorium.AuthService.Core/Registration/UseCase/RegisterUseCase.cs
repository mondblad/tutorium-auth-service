using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Tutorium.AuthService.Core.Abstractions;
using Tutorium.AuthService.Core.Exceptions;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models;

namespace Tutorium.AuthService.Core.Registration.UseCase
{
    internal class RegisterUseCase : IRegisterUseCase
    {
        private readonly IUserGrpcClient _userGrpcClient;
        private readonly INotificationGrpcClient _notificationGrpcClient;
        private readonly IRegistrationAttemptRepository _registrationAttemptRepository;

        public RegisterUseCase(
            IUserGrpcClient userGrpcClient,
            INotificationGrpcClient notificationGrpcClient,
            IRegistrationAttemptRepository registrationAttemptRepository)
        {
            _userGrpcClient = userGrpcClient;
            _notificationGrpcClient = notificationGrpcClient;
            _registrationAttemptRepository = registrationAttemptRepository;
        }

        public async Task<Ulid> StartRegistration()
        {
            var attempt = new RegistrationAttempt();

            await _registrationAttemptRepository.Add(attempt);

            return attempt.Token;
        }

        public async Task UpdateRegistrationAttempt(RegistrationAttemptUpdateDto dto)
        {
            var attempt = await _registrationAttemptRepository.GetByTokenAsync(dto.Token);

            if (attempt is null)
                throw new RegistrationAttemptNotFoundException();

            attempt.Update(dto);

            await _registrationAttemptRepository.Update(attempt);
        }

        public async Task<RegistrationAttemptDto> GetRegistrationAttempt(Ulid token)
        {
            var attempt = await _registrationAttemptRepository.GetByTokenAsync(token);

            if (attempt is null)
                throw new RegistrationAttemptNotFoundException();

            return attempt.GetRegistrationAttemptDto();
        }

        public async Task<Ulid> StartRegistration(string email, string password)
        {
            /*var isUserExists = await _userGrpcClient.IsUserExistsAsync(email);

            if (isUserExists is true)
                throw new UserAlreadyExistsException(email); 
            
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var attempt = new RegistrationAttempt(email, passwordHash);
            await _registrationAttemptRepository.test(attempt);

            await _notificationGrpcClient.SendEmailVerificationCodeAsync(attempt.Email, attempt.ConfirmationCode);*/

            return Ulid.Empty; //attempt.Token;
        }

        public async Task ConfirmRegistration(Ulid token, string code)
        {
            var attempt = await _registrationAttemptRepository.GetByTokenAsync(token);

            if (attempt is null)
                throw new RegistrationAttemptNotFoundException();
            else if (attempt.ConfirmationCode != code)
                throw new InvalidConfirmationCodeException();

            await _userGrpcClient.CreateUserAsync(attempt.Email, attempt.PasswordHash, attempt.CreatedAtUtc);
        }
    }
}
