using System.Collections.Generic;

namespace Hcs.Platform.Security
{
    public interface IUserPlatformRoleAccessor
    {
        IReadOnlyCollection<PlatformModule.IPlatformRole> Roles { get; }
    }
}