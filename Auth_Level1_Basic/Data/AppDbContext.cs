using Microsoft.EntityFrameworkCore;
using Auth_Level1_Basic.Models;

namespace Auth_Level1_Basic.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed a default user
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "123", Role = "Admin" }
        );
    }
}
