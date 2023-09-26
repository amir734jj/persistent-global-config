using Microsoft.EntityFrameworkCore;

namespace Core.Tests;

public sealed class EntityDbContext : DbContext
{
    public DbSet<GlobalConfig> GlobalConfigs { get; set; }
    
    public EntityDbContext(DbContextOptions<EntityDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }
}