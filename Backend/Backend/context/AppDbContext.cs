using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Backend.Configurations;
namespace Backend

{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<DealEntity> Deals { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DealConfiguration());
            
        
        }
    }
}
