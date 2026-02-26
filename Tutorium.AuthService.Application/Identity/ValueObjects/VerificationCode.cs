using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Tutorium.AuthService.Application.Identity.ValueObjects
{
    public sealed class VerificationCode : IEquatable<VerificationCode>
    {
        public string Value { get; init; }

        [JsonConstructor]
        public VerificationCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Code cannot be empty.", nameof(value));

            if (!IsValid(value))
                throw new ArgumentException("Code format is invalid.", nameof(value));

            Value = value;
        }

        public static VerificationCode Create()
        {
            return new VerificationCode(GenerateConfirmationCode());
        }

        public static VerificationCode CreateFromString(string code)
        {
            return new VerificationCode(code);
        }

        private static string GenerateConfirmationCode()
        {
            return RandomNumberGenerator.GetInt32(100_000, 1_000_000).ToString();
        }

        private static bool IsValid(string code)
        {
            return Regex.IsMatch(code, @"^\d{6}$");
        }

        public bool Equals(VerificationCode? other)
        {
            if (other is null) return false;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj) => Equals(obj as VerificationCode);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;
    }
}
