using Microsoft.IdentityModel.Tokens;

namespace Hcs.Platform
{
    public interface IJwtConfig
    {
        string Issuer { get; }
        string Audience { get; }
        SigningCredentials SigningCredentials { get; }
        int IssueDays { get; }
    }

}