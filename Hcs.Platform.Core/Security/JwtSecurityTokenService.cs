using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hcs.Platform.Core;
using Hcs.Platform.User;
using Microsoft.IdentityModel.Tokens;

namespace Hcs.Platform.Security
{
    internal class JwtSecurityTokenService : IJwtSecurityTokenService
    {
        private readonly PlatformConfigContext platformConfigContext;
        private readonly IUserRoleService userRoleService;

        public JwtSecurityTokenService(PlatformConfigContext platformConfigContext, IUserRoleService userRoleService)
        {
            this.platformConfigContext = platformConfigContext;
            this.userRoleService = userRoleService;
        }
        public async Task<JwtSecurityToken> CreateToken(IPlatformUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName,user.Name),
                new Claim(JwtRegisteredClaimNames.Jti,user.Id.ToString())
            };
            var roles = (await userRoleService.GetRoleCodes(user)).Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roles);
            var token = new JwtSecurityToken
            (
                issuer: platformConfigContext.JwtConfig.Issuer,
                audience: platformConfigContext.JwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(platformConfigContext.JwtConfig.IssueDays),
                signingCredentials: platformConfigContext.JwtConfig.SigningCredentials,
                notBefore: DateTime.UtcNow
            );
            return token;
        }
    }
}