using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hcs.Encryption
{
    public static class MD5
    {
        public static string Hash(string source)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(source)));
            }
        }
    }
}
