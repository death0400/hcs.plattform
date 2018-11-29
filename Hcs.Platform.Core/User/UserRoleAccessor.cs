using System;
using System.Collections.Generic;
using System.Linq;
using Hcs.Platform.Security;
using Hcs.Platform.User;

namespace Hcs.Platform.User
{
    class UserRoleAccessor : IUserRoleAccessor
    {
        Lazy<string[]> roles;
        public UserRoleAccessor(IPlatformUser user, IUserRoleService roleService)
        {
            roles = new Lazy<string[]>(() => roleService.GetRoleCodes(user).Result.ToArray());
        }
        public IEnumerable<string> Roles => roles.Value;
    }
}
