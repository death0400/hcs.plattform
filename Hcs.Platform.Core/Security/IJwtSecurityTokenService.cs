using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Hcs.Platform.Security
{
    public interface IJwtSecurityTokenService
    {
        Task<JwtSecurityToken> CreateToken(User.IPlatformUser user);
    }
}