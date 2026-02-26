using Microsoft.EntityFrameworkCore;
using Tutorium.AuthService.Application.Identity.Abstractions;
using Tutorium.AuthService.Core.Identity;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Infrastructure.Postgres.Repositories
{
    public class UserAuthenticationRepositories : IUserRepository
    {
        public readonly PgContext _context;

        public UserAuthenticationRepositories(PgContext context)
            => (_context) = (context);

        public async Task Add(UserAuthentication userAuthentication)
        {
            _context.UserAuthentications.Add(userAuthentication);
            await _context.SaveChangesAsync();
        }

        public bool ExistsByEmail(Email email)
        {
            return _context.EmailAuthentications.Any(t => t.Email.Value == email.Value);
        }

        public async Task<UserAuthentication?> FindByEmailAsync(Email email)
        {
            return await _context.UserAuthentications
                .AsNoTracking()
                .FirstOrDefaultAsync(t => 
                    t.ByEmail != null 
                    && t.ByEmail!.Email.Value == email.Value
                );
        }
    }
}
