using Microsoft.IdentityModel.Tokens;

namespace Hcs.Platform
{
    public class JwtConfigBuilder : IJwtConfig
    {
        internal JwtConfigBuilder()
        {

        }
        public string Issuer { get; set; } = "hcs.platform";
        public string Audience { get; set; } = "hcs.platform";
        public string IssuerSigningKey { get; set; } = "9315532e-8003-46cc-84c6-03e239cf4f3d";
        public int IssueDays { get; set; } = 10;
        public SigningCredentials SigningCredentials { get; set; }
    }

}