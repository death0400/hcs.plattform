using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hcs.Platform.Abstractions.Role;
using Hcs.Platform.Data;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.Security;
using Hcs.Platform.User;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Hcs.Platform.Role
{
    public class UserRoleService : IUserRoleService
    {
        private readonly PlatformDbContext dbcontext;
        private readonly IMemoryCache cache;
        public UserRoleService(PlatformDbContext dbcontext, IMemoryCache cache)
        {
            this.cache = cache;
            this.dbcontext = dbcontext;

        }

        public async Task<IEnumerable<string>> GetRoleCodes(IPlatformUser user)
        {
            var set = dbcontext.Set<BaseModels.PlatformGroupRole>();
            var key = Core.CacheKeyBuilder.UserRoleCode(user.Id);
            var roles = cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = new TimeSpan(0, 10, 0);
                var r = set.Where(x => x.PlatformGroup.PlatformUserGroups.Any(y => y.PlatformUserId == user.Id) && x.Permission != PermissionStatus.NotSet).AsEnumerable().Select(x => new { Permission = x.Permission, Code = x.FunctionCode + "." + x.FunctionRoleCode }).ToArray();
                var denied = r.Where(x => x.Permission == PermissionStatus.Denied).Select(x => x.Code).ToArray();
                var roleList = new List<string>(r.Where(x => x.Permission == PermissionStatus.Granted && !denied.Contains(x.Code)).Select(x => x.Code));
                roleList.Add("Everyone.All");
                return roleList;
            });
            return await Task.FromResult(roles);
        }
    }
}
