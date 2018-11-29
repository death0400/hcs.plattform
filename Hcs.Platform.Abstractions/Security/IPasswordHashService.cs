using System;

namespace Hcs.Platform.Security
{
    public interface IPasswordHashService
    {
        string Hash(string value);
    }
}
