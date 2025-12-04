using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(x => x.Id);

            builder.Property(m  => m.Id)
                .HasColumnName("message_id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(m => m.DealId)
                .HasColumnName("deal_id")
                .IsRequired();

            builder.Property(m => m.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(m => m.Text)
                .HasColumnName("text")
                .HasMaxLength(600)
                .IsRequired();

            builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

            builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at");

            builder.Property(m => m.DeletedAt)
            .HasColumnName("deleted_at");

            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.DealId);
        }
    }
}
