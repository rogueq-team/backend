using Backend.Entities;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.ToTable("applications");

            builder.HasKey(a => a.ApplicationId);

            builder.Property(a => a.ApplicationId)
                .HasColumnName("application_id")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(a => a.Cost)
                .HasColumnName("cost")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasConversion<string>()
                .HasDefaultValue(ApplicationStatus.InProgress)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(a => a.DeletedAt)
                .HasColumnName("deleted_at");

            builder.HasOne<UserEntity>()
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Deals)
                .WithOne(d => d.Application)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            /*builder.HasMany(a => a.Categories)
                .WithMany(c => c.ApplicationEntities)
                .UsingEntity<ApplicationCategoryEntity>(
                    j => j
                        .HasOne(ac => ac.Category)
                        .WithMany(c => c.ApplicationCategories)
                        .HasForeignKey(ac => ac.CategoryId),
                    j => j
                        .HasOne(ac => ac.Application)
                        .WithMany(a => a.ApplicationCategories)
                        .HasForeignKey(ac => ac.ApplicationId),
                    j =>
                    {
                        j.ToTable("application_categories");
                        j.HasKey(ac => new { ac.ApplicationId, ac.CategoryId });
                    }
                );*/

        }
    }
}
