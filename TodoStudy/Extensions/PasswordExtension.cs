using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace TodoStudy.Extensions
{
    public static class PasswordExtension
    {
        private const string _salt = "jisuStudyS@lt";

        public static string ToPasswordHash(this string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var str = $"{_salt}{password}";
                var stringByte = Encoding.Default.GetBytes(str);
                var computedHash = sha256.ComputeHash(stringByte, 0, stringByte.Length);
                var hashString = Convert.ToBase64String(computedHash);

                return hashString;       
             }
        }
    }
}
