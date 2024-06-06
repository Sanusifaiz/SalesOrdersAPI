using System.Security.Cryptography;
using System.Text;

namespace SalesOrders.Utilities.Utilities
{
    public class Utils
    {
        /// <summary>
        /// Password hash method, using SHA256 algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
