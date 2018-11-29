using Microsoft.EntityFrameworkCore;

namespace Hcs.Platform.Data
{
    public interface IModelConfig
    {
        void BuildSeedData(DbContext context);
        void BuildModel(ModelBuilder modelBuilder);
    }
}