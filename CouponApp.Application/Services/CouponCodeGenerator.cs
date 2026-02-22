using CouponApp.Application.Interfaces;
using System.Security.Cryptography;

namespace CouponApp.Application.Services
{
    public class CouponCodeGenerator : ICouponCodeGenerator
    {
        private const string Alphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

        public string Generate(int length = 10)
        {
            Span<char> chars = stackalloc char[length];
            var bytes = RandomNumberGenerator.GetBytes(length);

            for (int i = 0; i < length; i++)
            {
                chars[i] = Alphabet[bytes[i] % Alphabet.Length];
            }

            return new string(chars);
        }
    }
}
