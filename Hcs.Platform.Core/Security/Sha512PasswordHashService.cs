using System;

namespace Hcs.Platform.Security
{
    public class Sha512PasswordHashService : IPasswordHashService
    {
        public string Hash(string value)
        {
            return Hcs.Encryption.SHA.HashSha512(value);
        }
    }
}
