using System.Text.RegularExpressions;

namespace Farola.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be empty", nameof(value));
            var cleaned = Regex.Replace(value, @"[^\d+]", "");
            if (cleaned.Length < 5)
                throw new ArgumentException("Invalid phone number format", nameof(value));
            Value = value;
        }

        public static implicit operator string(PhoneNumber phone) => phone.Value;
        public static implicit operator PhoneNumber(string value) => new PhoneNumber(value);
    }
}
