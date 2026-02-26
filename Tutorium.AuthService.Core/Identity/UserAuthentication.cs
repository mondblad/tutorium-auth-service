using Tutorium.AuthService.Core.Identity.Entities;
using Tutorium.Shared.Utils.BaseModel;

namespace Tutorium.AuthService.Core.Identity
{
    public partial class UserAuthentication : BaseModelWithSoftDelete
    {
        public int UserId { get; protected set; }
        
        public EmailAuthentication? ByEmail { get; protected set; }

        protected UserAuthentication() { }

        private UserAuthentication(int userId, EmailAuthentication emailAuthentication)
            => (UserId, ByEmail) = (userId, emailAuthentication);
    }
}
