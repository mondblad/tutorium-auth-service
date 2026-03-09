using System.Security.Cryptography;
using System.Text;
using Tutorium.AuthService.Application.Sessions.Abstractions;

namespace Tutorium.AuthService.Infrastructure.Security
{
    internal class SessionIdGenerator : ISessionIdGenerator
    {
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public string GenerateSessionId()
        {
            int byteLength = 32;
            var bytes = new byte[byteLength];
            RandomNumberGenerator.Fill(bytes);

            var sb = new StringBuilder(byteLength * 2);
            foreach (var b in bytes)
            {
                sb.Append(Base62Chars[b % 62]);
            }

            return sb.ToString();
        }
    }
}
