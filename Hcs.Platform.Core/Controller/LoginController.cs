using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Hcs.Platform.Security;
using Hcs.Platform.User;
using Hcs.Platform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Hcs.Platform.Core.Controller
{
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtSecurityTokenService jwtSecurityTokenService;
        private readonly IMemoryCache cache;
        private readonly IUserRoleService userRole;

        public LoginController(IUserService userService, IJwtSecurityTokenService jwtSecurityTokenService, IMemoryCache cache, IUserRoleService userRole)
        {
            this.userRole = userRole;
            this.userService = userService;
            this.jwtSecurityTokenService = jwtSecurityTokenService;
            this.cache = cache;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginView login)
        {
            var result = await userService.CheckLogin(login);
            if (result.Successed)
            {
                var token = await jwtSecurityTokenService.CreateToken(result.User);
                foreach (var k in CacheKeyBuilder.GetAllKeys(result.User.Id))
                {
                    cache.Remove(k);
                }
                return Ok(new LoginResult { User = result.User, Succeeded = true, MessageCode = result.MessageCode, Token = new JwtSecurityTokenHandler().WriteToken(token), Roles = (await userRole.GetRoleCodes(result.User)).ToArray() });
            }
            else
            {
                return BadRequest(new LoginResult { Succeeded = false, MessageCode = result.MessageCode });
            }
        }
    }
}
