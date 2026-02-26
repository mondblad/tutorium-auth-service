namespace Tutorium.AuthService.Application.Identity.Abstractions.Security
{
    public interface IPasswordValidator
    {
        void Validate(string password);
    }
}
