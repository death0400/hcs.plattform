using System.Linq;
using Hcs.Platform.Odata;
using Hcs.Platform.Security;
using Hcs.Platform.User;
using Microsoft.Extensions.Caching.Memory;

namespace Hcs.Platform.Security
{
    public class UserOdataPermissionService : IUserOdataPermissionService
    {
        public UserOdataPermissionService(IPlatformUser user, IUserPlatformRoleAccessor roles, IMemoryCache cache)
        {
            var key = Core.CacheKeyBuilder.UserOdataPermission(user.Id);
            OdataQueryPermission = cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = new System.TimeSpan(0, 10, 0);
                return new CombinedOdataQueryPermission(roles.Roles.SelectMany(x => x.QueryPermissions).ToArray());
            });
        }

        public Odata.IOdataQueryPermission OdataQueryPermission { get; }
    }
}
