using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("transaction_id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(t => t.DealId)
                .HasColumnName("deal_id")
                .IsRequired();

            builder.Property(t => t.Amount)
                .HasColumnName("amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

            builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

            builder.Property(t => t.DeletedAt)
            .HasColumnName("deleted_at");

            builder.HasIndex(t => t.Id);
        }
    }
}
