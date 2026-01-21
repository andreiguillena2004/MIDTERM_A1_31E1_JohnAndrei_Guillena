using Microsoft.EntityFrameworkCore;
using Auth_Level4_RBAC.Models;

namespace Auth_Level4_RBAC.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "123", Role = "Admin" },
            new User { Id = 2, Username = "bob", Password = "123", Role = "User" }
        );
    }
}
