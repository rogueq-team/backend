using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class AdvertiserCategoryConfiguration : IEntityTypeConfiguration<AdvertiserCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<AdvertiserCategoryEntity> builder)
        {
            builder.ToTable("advertiser_categories");

            builder.HasKey(ac => new { ac.AdvertiserId, ac.CategoryId });

            builder.Property(ac => ac.AdvertiserId)
                .HasColumnName("advertiser_id")
                .IsRequired();

            builder.Property(ac => ac.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.HasOne(ac => ac.Advertiser)
                .WithMany(u => u.AdvertiserCategories)
                .HasForeignKey(ac => ac.AdvertiserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ac => ac.Category)
                .WithMany(u => u.AdvertiserCategories)
                .HasForeignKey(ac => ac.AdvertiserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ac => ac.AdvertiserId);
            builder.HasIndex(ac => ac.CategoryId);
        }
    }
}
