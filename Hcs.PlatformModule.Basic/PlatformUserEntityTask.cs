using System.Threading.Tasks;
using Hcs.Platform.Data;
using Hcs.Platform.BaseModels;

namespace Hcs.PlatformModule.Basic
{
    public class PlatformUserEntityTask : DataProcessorEntityTask<PlatformUser>
    {
        public override async Task PreUpdate(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<PlatformUser> entity)
        {
            if (entity.Entity.Password == null)
            {
                entity.Property(e => e.Password).IsModified = false;
            }
            await Task.CompletedTask;
        }
    }
}
