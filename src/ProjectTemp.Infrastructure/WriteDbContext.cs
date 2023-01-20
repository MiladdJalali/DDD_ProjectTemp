using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTemp.Domain.Aggregates.Users;

namespace ProjectTemp.Infrastructure;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}

#if DEBUG
public class DesignTimeResourceDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
{
    public WriteDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<WriteDbContext>();

        builder.UseNpgsql("Username=U;Password=P;Database=D;Host=127.0.0.1");

        return new WriteDbContext(builder.Options);
    }
}
#endif