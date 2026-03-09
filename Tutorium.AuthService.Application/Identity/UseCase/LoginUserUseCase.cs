using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Application.Identity.Abstractions.Security;
using Tutorium.AuthService.Application.Identity.Abstractions.UseCases;
using Tutorium.AuthService.Application.Sessions.Abstractions;
using Tutorium.AuthService.Core.Sessions.Models;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Application.Identity.UseCase
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISessionManager _sessionManager;

        public LoginUserUseCase(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHasher passwordHasher,
            ISessionManager sessionManager)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
            _sessionManager = sessionManager;
        }

        public async Task<Session> AuthenticateAsync(Email email, string password)
        {
            // Ищем пользователя по email
            var user = await _userRepository.FindByEmailAsync(email);
            if (user is null)
                throw new Exception("Пользователь с таким email не найден");

            if (user.ByEmail is null)
                throw new Exception("");

            // Проверяем пароль
            if (!_passwordHasher.Verify(password, user.ByEmail.PasswordHash))
                throw new Exception("Неверный пароль");

            // Генерируем JWT
            //var token = _jwtTokenService.GenerateToken(user.Id);
            return await _sessionManager.CreateSessionAsync(user.Id);
        }
    }
}
