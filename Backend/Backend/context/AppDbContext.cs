using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace Backend
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.UserId);
                

                entity.Property(e => e.Role)
                    .HasConversion<string>() 
                    .IsRequired();
                    
                entity.Property(e => e.Type)
                    .HasConversion<string>() 
                    .IsRequired();


                entity.HasIndex(e => e.Login).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                
                entity.Property(e => e.SocialLinks)
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => v == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                // Автоматические даты
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                    
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Длина полей
                entity.Property(e => e.Name)
                    .HasMaxLength(100);
                    
                entity.Property(e => e.Login)
                    .HasMaxLength(30);
                    
                entity.Property(e => e.Email)
                    .HasMaxLength(255);
                    
                entity.Property(e => e.Password)
                    .HasMaxLength(255);
                    
                entity.Property(e => e.Bio)
                    .HasMaxLength(600);
            });
        }
    }
}
