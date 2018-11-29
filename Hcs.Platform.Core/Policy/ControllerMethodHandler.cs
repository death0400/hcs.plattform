using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Hcs.Platform.Role;
using System.Collections.Generic;
using Hcs.Platform.Security;

namespace Hcs.Platform.Policy
{
    public class RoleTokenHandler : AuthorizationHandler<ControllerMethodRequirement, Role.IRoleToken>
    {
        IReadOnlyDictionary<string, HashSet<string>> platformPermissionToRoleMap;
        public RoleTokenHandler(IUserPlatformRoleAccessor userRoles)
        {
            if (userRoles.Roles != null)
            {
                var map = new Dictionary<string, HashSet<string>>();
                foreach (var pp in userRoles.Roles.Select(p => new { p.Code, p.Roles }))
                {
                    map.AddIfNotExists(pp.Code, () => new HashSet<string>());
                    map[pp.Code] = new HashSet<string>(map[pp.Code].Concat(pp.Roles).Distinct());
                }
                platformPermissionToRoleMap = map;
            }
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ControllerMethodRequirement requirement, IRoleToken resource)
        {
            if (platformPermissionToRoleMap != null)
            {
                foreach (var r in context.User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role))
                {
                    if (platformPermissionToRoleMap.ContainsKey(r.Value) && platformPermissionToRoleMap[r.Value].Contains(resource.Name))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
