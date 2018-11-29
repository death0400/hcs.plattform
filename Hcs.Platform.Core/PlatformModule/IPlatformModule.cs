using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform.PlatformModule
{
    public interface IPlatformModule
    {
        void Build(IPlatformModuleBuilder moduleBuilder);
    }
}