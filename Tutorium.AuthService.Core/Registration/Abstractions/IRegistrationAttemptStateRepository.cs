using Tutorium.AuthService.Core.Registration.Models.RegistrationAttempt;
using Tutorium.Shared.Utils.EntityFramework.Abstractions;

namespace Tutorium.AuthService.Core.Registration.Abstractions
{
    public interface IRegistrationAttemptStateRepository : IPostgresCRUDRepository<RegistrationAttemptState> 
    {
    }
}
