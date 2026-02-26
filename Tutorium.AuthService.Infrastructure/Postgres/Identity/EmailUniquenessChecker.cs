using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Core.Identity.Abstractions;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Infrastructure.Postgres.Identity
{
    internal class EmailUniquenessChecker : IEmailUniquenessChecker
    {
        private readonly IUserRepository _userRepository;

        public EmailUniquenessChecker(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsUnique(Email email)
        {
            return !_userRepository.ExistsByEmail(email);
        }
    }
}
