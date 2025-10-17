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
                .HasColumnType("jsonb");

            builder.Property(u => u.IsVerified)
                .HasColumnName("is_verified")
                .IsRequired();

            //это на потом, тут будут связи с другими таблицами

            //builder.HasMany(u => u.Applications)
            //    .WithOne() //позже
            //    .HasForeignKey("user_id");

            //builder.HasMany(u => u.Deals)
            //    .WithOne() //позже
            //    .HasForeignKey("user_id");

            //builder.HasMany(u => u.AdvertiserCategories)
            //    .WithMany() //позже
            //    .UsingEntity(j => j.ToTable("advertiser_categories"));

            //builder.HasMany(u => u.PlatformCategories)
            //    .WithMany() //позже
            //    .UsingEntity(j => j.ToTable("platform_categories"));
        }
    }
}
