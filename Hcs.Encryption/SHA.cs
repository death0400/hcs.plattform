using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hcs.Encryption
{
    public static class SHA
    {
        public static string HashSha1(string source, Encoding encoding = null)
        {
            var useEncoding = encoding ?? Encoding.UTF8;
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return Hex.ConvertToHexString(sha1.ComputeHash(useEncoding.GetBytes(source)));
            }
        }
        public static string HashSha512(string source, Encoding encoding = null)
        {
            var useEncoding = encoding ?? Encoding.UTF8;
            using (var sha512 = new SHA512CryptoServiceProvider())
            {
                return Hex.ConvertToHexString(sha512.ComputeHash(useEncoding.GetBytes(source)));
            }
        }
    }
}
