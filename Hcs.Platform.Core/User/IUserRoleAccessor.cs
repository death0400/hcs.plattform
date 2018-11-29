using System.Collections.Generic;

namespace Hcs.Platform.User
{
    public interface IUserRoleAccessor
    {
        IEnumerable<string> Roles { get; }
    }
}
