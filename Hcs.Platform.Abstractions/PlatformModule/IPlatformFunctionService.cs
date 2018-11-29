using System.Collections.Generic;
namespace Hcs.Platform.PlatformModule
{
    public interface IPlatformFunctionService
    {
        IEnumerable<IPlatformFunction> Functions { get; }
    }
}
