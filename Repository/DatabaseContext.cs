using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ProfileService.Model.Entity;

namespace ProfileService.Repository;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SomethingEntity> Somethings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SomethingEntity>().ToTable("somethings");
    }
}