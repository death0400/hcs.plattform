using System.Collections.Generic;
using System.Threading.Tasks;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.User;

namespace Hcs.Platform.Security
{
    public interface IUserRoleService
    {
        Task<IEnumerable<string>> GetRoleCodes(IPlatformUser user);
    }
}