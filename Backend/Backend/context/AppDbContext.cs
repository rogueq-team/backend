using System.Text.Json;
using Backend.Configurations;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.context
{
    public class AppDbContext : DbContext
    {
        public DbSet<ApplicationEntity> Applications { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        }
    }
}