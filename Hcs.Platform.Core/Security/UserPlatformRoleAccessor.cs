using System.Collections.Generic;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.User;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace Hcs.Platform.Security
{
    public class UserPlatformRoleAccessor : IUserPlatformRoleAccessor
    {
        IPlatformRole[] roles;
        public IReadOnlyCollection<IPlatformRole> Roles => roles;
        public UserPlatformRoleAccessor(IPlatformUser user, IUserRoleService userRole, IPlatformFunctionService functionService, IMemoryCache cache)
        {
            if (user != null)
            {
                var key = Core.CacheKeyBuilder.UserRole(user.Id);
                roles = cache.GetOrCreate(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = new System.TimeSpan(0, 10, 0);
                    var roleCodes = userRole.GetRoleCodes(user).Result;
                    return functionService.Functions.SelectMany(x => x.Permissions).Where(x => roleCodes.Contains(x.Code)).ToArray();
                });
            }
        }
    }
}