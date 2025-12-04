using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class PlatformCategoryConfiguration : IEntityTypeConfiguration<PlatformCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<PlatformCategoryEntity> builder)
        {
            builder.ToTable("platform_categories");

            builder.HasKey(pc => new { pc.PlatformId, pc.CategoryId });

            builder.Property(ac => ac.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(ac => ac.PlatformId)
                .HasColumnName("platform_id")
                .IsRequired();

            builder.HasOne(ac => ac.Category)
                .WithMany(u => u.PlatformsCategories)
                .HasForeignKey(pc => pc.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ac => ac.Platform)
                .WithMany(u => u.PlatformCategories)
                .HasForeignKey(pc => pc.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pc => pc.PlatformId);
            builder.HasIndex(pc => pc.CategoryId);
        }
    }
}
