using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Application.Identity.Abstractions.Security;
using Tutorium.AuthService.Application.Identity.Abstractions.UseCases;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Application.Identity.UseCase
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserUseCase(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> AuthenticateAsync(Email email, string password)
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
            var token = _jwtTokenService.GenerateToken(user.Id);
            return token;
        }
    }
}
