using crud_app.Domain;
using Microsoft.EntityFrameworkCore;

namespace crud_app.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
                    : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
