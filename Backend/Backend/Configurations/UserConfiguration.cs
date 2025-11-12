using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder) 
        {
            builder.ToTable("users");

            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Login)
                .HasColumnName("login")
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(u => u.Login).IsUnique();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(u => u.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(u => u.Balance)
                .HasColumnName("balance")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(u => u.AvatarPath)
                .HasColumnName("avatar_path");

            builder.Property(u => u.Bio)
                .HasColumnName("bio")
                .HasMaxLength(600);

            builder.Property(u => u.SocialLinks)
                .HasColumnName("social_links")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => v == null ? new List<string>() : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property(u => u.IsVerified)
                .HasColumnName("is_verified")
                .IsRequired();

            builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();


            builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at");

            builder.Property(u => u.DeletedAt)
            .HasColumnName("deleted_at");
            //это на потом, тут будут связи с другими таблицами

            builder.HasMany(u => u.Applications)
                .WithOne()
                .HasForeignKey(a => a.UserId);

           /* builder.HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(a => a.UserId);*/

            builder.HasMany(u => u.SentFeedbacks)
                .WithOne(f => f.Sender)
                .HasForeignKey(a => a.SenderId);

            builder.HasMany(u => u.ReceivedFeedbacks)
                .WithOne(f => f.Recipient)
                .HasForeignKey(a => a.RecipientId);

            builder.HasMany(u => u.DealsAsAdvertiser)
            .WithOne(d => d.Advertiser)
            .HasForeignKey(d => d.AdvertiserId)
            .OnDelete(DeleteBehavior.Restrict); // Чтобы не удалялись сделки при удалении пользователя

            builder.HasMany(u => u.DealsAsPlatform)
            .WithOne(d => d.Platform)
            .HasForeignKey(d => d.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

            /*builder.HasMany(u => u.AdvertiserCategories)
                .WithOne(c => c.Advertiser)
                .HasForeignKey(c => c.AdvertiserId);*/

            /*builder.HasMany(u => u.PlatformCategories)
                .WithOne(c => c.Platform)
                .HasForeignKey(c => c.PlatformId);
                */
        }
    }
}
