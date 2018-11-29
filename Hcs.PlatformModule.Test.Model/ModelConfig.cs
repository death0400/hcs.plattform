using System;
using Microsoft.EntityFrameworkCore;
using Hcs.Platform.Data;
namespace Hcs.PlatformModule.Test.Model
{
    public class ModelConfig : IModelConfig
    {
        public void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Customer>(entity =>
            {
                entity.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(x => x.LastUpdatedByUser).WithMany().HasForeignKey(x => x.LastUpdatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasMany(x => x.Orders).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Entities.Order>(entity =>
            {
                entity.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(x => x.LastUpdatedByUser).WithMany().HasForeignKey(x => x.LastUpdatedBy).OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        public void BuildSeedData(DbContext context)
        {

        }
    }
}
