using Tutorium.AuthService.Application.Identity.Runtime;
using Tutorium.AuthService.Application.Identity.ValueObjects;
using Tutorium.AuthService.Core.Identity.ValueObjects;
using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.AuthService.Core.Abstractions;
using Tutorium.AuthService.Core.Identity;
using Tutorium.AuthService.Core.Identity.Abstractions;
using Tutorium.AuthService.Application.Identity.Abstractions.UseCases;
using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Application.Identity.Abstractions.Security;
using Newtonsoft.Json.Linq;
using Tutorium.AuthService.Core.Sessions.Models;
using Tutorium.AuthService.Application.Sessions.Abstractions;

namespace Tutorium.AuthService.Application.Identity.UseCase
{
    public class RegistrationUseCase : IRegistrationUseCase
    {
        private readonly ISessionManager _sessionManager;

        private readonly IUserGrpcClient _userGrpcClient;
        private readonly INotificationGrpcClient _notificationGrpcClient;

        private readonly IPendingRegistrationRepository _pendingRegistrationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailUniquenessChecker _emailUniquenessChecker;

        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;

        private readonly IJwtTokenService _jwtTokenService;

        public RegistrationUseCase(IPendingRegistrationRepository pendingRegistrationRepository, IUserGrpcClient userGrpcClient,
            IUserRepository userRepository, IEmailUniquenessChecker emailUniquenessChecker, IPasswordHasher passwordHasher, 
            IPasswordValidator passwordValidator, INotificationGrpcClient notificationGrpcClient, IJwtTokenService jwtTokenService,
            ISessionManager sessionManager)
        {
            _pendingRegistrationRepository = pendingRegistrationRepository;
            _userGrpcClient = userGrpcClient;
            _userRepository = userRepository;
            _emailUniquenessChecker = emailUniquenessChecker;
            _notificationGrpcClient = notificationGrpcClient;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _jwtTokenService = jwtTokenService;
            _sessionManager = sessionManager;
        }
        
        public async Task<Guid> InitiateRegistrationAsync(Email email, string password)
        {
            _passwordValidator.Validate(password);
            var passwordHash = _passwordHasher.Hash(password);

            if (!_emailUniquenessChecker.IsUnique(email))
                throw new ArgumentException("Email must be unique");

            var code = VerificationCode.Create();

            await _notificationGrpcClient.SendEmailVerificationCodeAsync(email.Value, code.Value);

            var runtime = new PendingRegistration(email, passwordHash, code);

            await _pendingRegistrationRepository.Add(runtime);

            return runtime.Token;
        }

        public async Task<Session> ConfirmRegistrationAsync(Guid token, VerificationCode code)
        {
            var runtime = await _pendingRegistrationRepository.GetByTokenAsync(token);
            if (runtime is null)
                throw new NullReferenceException();

            if (!runtime.VerificationCode.Equals(code))
                throw new InvalidOperationException();

            var userId = await _userGrpcClient.CreateUserAsync(runtime.Email.Value, runtime.PasswordHash.Value, DateTime.UtcNow);

            var userAuthentication = UserAuthentication.CreateByEmailAuthentication(userId, runtime.Email, runtime.PasswordHash, _emailUniquenessChecker);

            await _userRepository.Add(userAuthentication);

            return await _sessionManager.CreateSessionAsync(userId);
        }

        public async Task<Email> GetEmailAsync(Guid token)
        {
            var runtime = await _pendingRegistrationRepository.GetByTokenAsync(token);
            if (runtime is null)
                throw new NullReferenceException();

            return runtime.Email;
        }
    }
}
