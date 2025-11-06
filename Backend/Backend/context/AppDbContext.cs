using System.Text.Json;
using Backend.Configurations;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
namespace Backend

{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<DealEntity> Deals { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<FeedbackEntity> Feedbacks { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<PlatformCategoryEntity> PlatformCategories { get; set; }
        public DbSet<AdvertiserCategoryEntity> AdvertiserCategories { get; set; }
        public DbSet<ApplicationCategoryEntity> ApplicationCategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DealConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());



        }
    }
}
