using Tutorium.AuthService.Core.Identity.ValueObjects;
using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.Shared.Utils.BaseModel;

namespace Tutorium.AuthService.Core.Identity.Entities
{
    public class EmailAuthentication : BaseModelWithSoftDelete
    {
        public Email Email { get; protected set; }
        public PasswordHash PasswordHash { get; protected set; }

        protected EmailAuthentication() { }

        private EmailAuthentication(Email email, PasswordHash passwordHash)
            => (Email, PasswordHash) = (email, passwordHash);

        public static EmailAuthentication Create(Email email, PasswordHash passwordHash)
        {
            return new EmailAuthentication(email, passwordHash);
        }
    }
}
