using Tutorium.AuthService.Core.Identity;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Application.Identity.Abstractions
{
    public interface IUserRepository
    {
        Task Add(UserAuthentication userAuthentication);
        bool ExistsByEmail(Email email);
        Task<UserAuthentication?> FindByEmailAsync(Email email);
    }
}
