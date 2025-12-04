using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("category_id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.Slug)
                .HasColumnName("slug")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.ParentCategoryId)
                .HasColumnName("parent_category_id");

            builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

            builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

            builder.Property(c => c.DeletedAt)
            .HasColumnName("deleted_at");

            builder.HasOne(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.PlatformsCategories)
                .WithOne(pc => pc.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.AdvertiserCategories)
                .WithOne(ac => ac.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ApplicationCategories)
                .WithOne(ac => ac.Category)
                .HasForeignKey(ac => ac.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.Slug).IsUnique();
            builder.HasIndex(c => c.Name);
        }
    }
}
