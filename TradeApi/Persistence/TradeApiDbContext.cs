using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Protocol;
using TradeApi.Domain;

namespace TradeApi.Persistence;

public class TradeApiDbContext : DbContext
{
    public TradeApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Trade> Trades { get; set; }
    public DbSet<Trader> Traders { get; set; }
    public DbSet<Share> Shares { get; set; }
    public DbSet<TraderPortfolio> TraderPortfolioes { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries().Where(w => w.Entity is ISoftDelete && w.State == EntityState.Deleted))
        {
            ((ISoftDelete)item.Entity).DeletedAt = DateTime.UtcNow;
            item.State = EntityState.Modified;
        }

        foreach (var item in ChangeTracker.Entries().Where(w => w.Entity is IActionLog && w.State == EntityState.Added))
        {
            ((IActionLog)item.Entity).CreatedAt = DateTime.UtcNow;
        }

        foreach (var item in ChangeTracker.Entries().Where(w => w.Entity is IActionLog && w.State == EntityState.Modified))
        {
            ((IActionLog)item.Entity).UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trade>().HasQueryFilter(w => w.DeletedAt == null);
        base.OnModelCreating(modelBuilder);
    }
}
