using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class ApplicationCategoryConfiguration : IEntityTypeConfiguration<ApplicationCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationCategoryEntity> builder)
        {
            builder.ToTable("application_categories");

            builder.HasKey(ac => new {ac.ApplicationId, ac.CategoryId });

            builder.Property(ac => ac.ApplicationId)
                .HasColumnName("application_id")
                .IsRequired();

            builder.Property(ac => ac.CategoryId)
                .HasColumnName("category_od")
                .IsRequired();

            builder.HasOne(ac => ac.Application)
                .WithMany(a => a.ApplicationCategories)
                .HasForeignKey(ac => ac.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ac => ac.Category)
                .WithMany(c => c.ApplicationCategories)
                .HasForeignKey(ac => ac.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ac => ac.ApplicationId);
            builder.HasIndex(ac => ac.CategoryId);
        }
    }
}
