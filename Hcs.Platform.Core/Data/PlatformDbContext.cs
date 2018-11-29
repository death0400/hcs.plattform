using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.Data
{
    public class PlatformDbContext : DbContext
    {
        private readonly IDbModelCreatingConfig configuraions;

        public PlatformDbContext(IDbModelCreatingConfig configuraions, DbContextOptions<PlatformDbContext> options) : base(options)
        {
            this.configuraions = configuraions;
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var config in configuraions.Configuraions)
            {
                config(modelbuilder);
            }
        }
    }
}