using System.Text.Json.Serialization;
using BCryptAlias = BCrypt.Net.BCrypt;

namespace Tutorium.AuthService.Core.Identity.ValueObjects
{
    public sealed class PasswordHash : IEquatable<PasswordHash>
    {
        public string Value { get; init; }

        [JsonConstructor]
        private PasswordHash(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static PasswordHash Create(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash cannot be empty.", nameof(hash));

            return new PasswordHash(hash);
        }

        public bool Equals(PasswordHash? other)
        {
            if (other is null)
                return false;

            return Value == other.Value;
        }

        public override bool Equals(object? obj) => Equals(obj as PasswordHash);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;
    }
}
