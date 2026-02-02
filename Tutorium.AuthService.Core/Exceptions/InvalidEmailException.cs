using Tutorium.Shared.Utils.Exceptions;

namespace Tutorium.AuthService.Core.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException() : base("Email format is invalid") { }
    }
}
