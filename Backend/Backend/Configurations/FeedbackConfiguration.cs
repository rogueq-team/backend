using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<FeedbackEntity>
    {
        public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
        {
            builder.ToTable("feedbacks");

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id)
                .HasColumnName("feedback_id")
                .ValueGeneratedOnAdd();

            builder.Property(f => f.DealId)
                .HasColumnName("deal_id")
                .IsRequired();

            builder.Property(f => f.Text)
                .HasColumnName("text")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Stars)
                .HasColumnName("stars")
                .IsRequired()
                .HasAnnotation("Range", new[] { 0, 5 });

            builder.Property(f => f.SenderId)
                .HasColumnName("sender_id")
                .IsRequired();

            builder.Property(f => f.RecipientId)
                .HasColumnName("recipient_id")
                .IsRequired();

            builder.Property(f => f.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

            builder.Property(f => f.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(f => f.DeletedAt)
                .HasColumnName("deleted_at");

            builder.HasOne(f => f.Sender)
                .WithMany(u => u.SentFeedbacks)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(f => f.Recipient)
                .WithMany(u => u.ReceivedFeedbacks)
                .HasForeignKey(f => f.RecipientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
