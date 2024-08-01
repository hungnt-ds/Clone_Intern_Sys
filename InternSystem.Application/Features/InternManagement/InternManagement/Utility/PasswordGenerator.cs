using System.Text;
using Microsoft.Extensions.Configuration;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Utility
{
    public static class PasswordGenerator
    {
        private static readonly Random Random = new Random();
        private static string? Uppercase;
        private static string? Lowercase;
        private static string? Digits;
        private static string? Special;
        public static void Initialize(IConfiguration configuration)
        {
            Uppercase = configuration["PasswordSettings:UppercaseChars"] ?? "";
            Lowercase = configuration["PasswordSettings:LowercaseChars"] ?? "";
            Digits = configuration["PasswordSettings:DigitChars"] ?? "";
            Special = configuration["PasswordSettings:SpecialChars"] ?? "";
        }

        public static string Generate(int length)
        {
            if (length < 6) throw new ArgumentException("Password length should be at least 6 characters.");

            var allChars = Uppercase + Lowercase + Digits + Special;
            var password = new StringBuilder();

            // Check password format
            password.Append(Uppercase[Random.Next(Uppercase.Length)]);
            password.Append(Lowercase[Random.Next(Lowercase.Length)]);
            password.Append(Digits[Random.Next(Digits.Length)]);
            password.Append(Special[Random.Next(Special.Length)]);

            // Fill the password length with random characters
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[Random.Next(allChars.Length)]);
            }

            // Randomize password
            return new string(password.ToString().ToCharArray().OrderBy(c => Random.Next()).ToArray());
        }
    }
}
