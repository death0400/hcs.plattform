using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hcs.Encryption
{
    public class AES
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        protected AesCryptoServiceProvider GetProvider()
        {
            using (var md5 = new MD5CryptoServiceProvider())
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                return new AesCryptoServiceProvider()
                {
                    Key = sha256.ComputeHash(Key),
                    IV = md5.ComputeHash(IV),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
            }
        }
        public byte[] Encrypt(byte[] data)
        {

            using (var provider = GetProvider())
            {
                using (var memStream = new MemoryStream())
                {
                    using (var encStream = new CryptoStream(memStream, provider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        encStream.Write(data, 0, data.Length);
                        encStream.FlushFinalBlock();
                        memStream.Position = 0;
                        return memStream.ToArray();
                    }
                }
            }
        }
        public string Encrypt(string data)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data)));
        }
        public byte[] Decrypt(byte[] data)
        {
            var md5 = new MD5CryptoServiceProvider();
            var sha256 = new SHA256CryptoServiceProvider();
            using (var provider = GetProvider())
            {
                using (var memStream = new MemoryStream())
                {
                    using (var encStream = new CryptoStream(memStream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        encStream.Write(data, 0, data.Length);
                        encStream.FlushFinalBlock();
                        memStream.Position = 0;
                        return memStream.ToArray();
                    }
                }
            }
        }
        public string Decrypt(string data)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data)));
        }
    }
}
