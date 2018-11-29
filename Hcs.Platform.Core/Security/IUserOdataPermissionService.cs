using System;
using Hcs.Platform.PlatformModule;

namespace Hcs.Platform.Security
{
    public interface IUserOdataPermissionService
    {
        Odata.IOdataQueryPermission OdataQueryPermission { get; }
    }
}
