using mednik.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace mednik.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Group> Groups { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Services> Services { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}