using System.Security.Cryptography;
using System.Text;

namespace auto_front.Lib
{
    public static class Crypto
    {
        public static string GenerateHash(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Compare(string password, string storedHash)
        {
            return GenerateHash(password) == storedHash;
        }
    }
}
