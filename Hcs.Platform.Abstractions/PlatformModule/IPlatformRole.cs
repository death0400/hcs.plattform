using System.Collections.Generic;
using Hcs.Platform.Odata;

namespace Hcs.Platform.PlatformModule
{
    public interface IPlatformRole
    {
        string PermissionCode { get; }
        /// <summary>
        /// full code (function+permission)
        /// </summary>
        /// <returns></returns>
        string Code { get; }
        IEnumerable<string> Roles { get; }
        IEnumerable<IOdataQueryPermission> QueryPermissions { get; }
    }
}