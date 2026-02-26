using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Core.Identity.Abstractions
{
    public interface IEmailUniquenessChecker
    {
        bool IsUnique(Email email);
    }
}
