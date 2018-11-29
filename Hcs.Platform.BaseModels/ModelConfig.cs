using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.BaseModels
{
    public class ModelConfig : Hcs.Platform.Data.IModelConfig
    {
        public void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlatformFile>(entity =>
            {
                entity.HasIndex(x => x.Key).IsUnique();
                entity.HasIndex(x => x.Dir).IsUnique(false);
            });
            modelBuilder.Entity<PlatformFlag>(entity =>
            {
                entity.HasKey(e => e.Flag);
                entity.Property(e => e.Flag).HasMaxLength(50);
            });
            modelBuilder.Entity<PlatformUser>(entity =>
            {
                entity.HasIndex(e => e.Account).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Account).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasMany(e => e.PlatformUserGroups).WithOne(e => e.PlatformUser).HasForeignKey(e => e.PlatformUserId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PlatformGroupRole>(entity =>
            {
                entity.Property(e => e.FunctionCode).HasMaxLength(200).IsRequired();
                entity.Property(e => e.FunctionCode).HasMaxLength(200).IsRequired();
                entity.Property(e => e.FunctionRoleCode).HasMaxLength(200).IsRequired();
                entity.HasOne(e => e.PlatformGroup).WithMany(e => e.PlatformGroupRoles).HasForeignKey(e => e.PlatformGroupId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedBy).OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<PlatformGroup>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.HasMany(e => e.PlatformUserGroups).WithOne(e => e.PlatformGroup).HasForeignKey(e => e.PlatformGroupId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedBy).OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<PlatformUserGroup>(entity =>
            {
                entity.HasKey(e => new { e.PlatformGroupId, e.PlatformUserId });
            });
        }

        public void BuildSeedData(DbContext context)
        {

        }
    }
}
