using System.Text.RegularExpressions;

namespace Farola.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty", nameof(value));
            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format", nameof(value));
            Value = value;
        }

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string value) => new Email(value);
    }
}
