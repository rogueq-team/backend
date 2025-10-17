using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
