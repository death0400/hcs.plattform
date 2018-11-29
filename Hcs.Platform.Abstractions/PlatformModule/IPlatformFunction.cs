using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hcs.Platform.PlatformModule;
namespace Hcs.Platform.PlatformModule
{
    public interface IPlatformFunction
    {
        [Key]
        string Code { get; }
        IEnumerable<IPlatformRole> Permissions { get; }
    }
}
