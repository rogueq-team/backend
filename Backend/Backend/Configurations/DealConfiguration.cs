using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class DealConfiguration : IEntityTypeConfiguration<DealEntity>
    {
        public void Configure(EntityTypeBuilder<DealEntity> builder) 
        {
            builder.ToTable("deals");

            builder.HasKey(d => d.DealId);
            builder.Property(d => d.DealId)
            .HasColumnName("deal_id") 
            .ValueGeneratedOnAdd();


            builder.Property(d => d.ApplicationId)
            .HasColumnName("application_id") 
            .IsRequired();

             builder.Property(d => d.AdvertiserId)
            .HasColumnName("advertiser_id") 
            .IsRequired();

            builder.Property(d => d.PlatformId)
            .HasColumnName("platform_id")
            .IsRequired();

            builder.Property(d => d.Description)
            .HasColumnName("description")
            .HasMaxLength(600);

            builder.Property(d => d.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(100);

            builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

            builder.Property(d => d.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired();

            builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

            builder.HasOne(d => d.Advertiser)
            .WithMany(u => u.DealsAsAdvertiser)
            .HasForeignKey(d => d.AdvertiserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            builder.HasOne(d => d.Platform)
            .WithMany(u => u.DealsAsPlatform)
            .HasForeignKey(d => d.PlatformId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            builder.HasOne(d => d.Transaction)
                .WithOne(t => t.Deal)
                .HasForeignKey<TransactionEntity>(t => t.DealId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Messages)
                .WithOne(m => m.Deal)
                .HasForeignKey(d => d.DealId);
            
            builder.HasIndex(d => d.AdvertiserId);
            builder.HasIndex(d => d.PlatformId);
            builder.HasIndex(d => d.ApplicationId);
        }
    }
}
