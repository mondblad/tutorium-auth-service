using System.Net.Mail;
using System.Text.Json.Serialization;

namespace Tutorium.AuthService.Core.Shared.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        public string Value { get; init; }

        [JsonConstructor]
        private Email(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid email format.", nameof(value));

            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (!IsValid(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            return new Email(email.Trim().ToLowerInvariant());
        }

        private static bool IsValid(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as Email);

        public bool Equals(Email? other)
        {
            if (other is null) 
                return false;

            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public override string ToString() => Value;
    }
}
