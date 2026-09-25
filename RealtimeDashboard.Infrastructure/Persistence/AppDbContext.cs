using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Metric> Metrics => Set<Metric>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Metric>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Key).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Label).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Unit).HasMaxLength(20);
            entity.HasIndex(m => m.Key).IsUnique();

            entity.HasData(
                CreateSeed("{7CCD0DC7-D73C-41B0-A734-222D00E25BDC}", "active_users", "Active users", "#", MetricCategory.Users),
                CreateSeed("{1CCD0DC7-D73C-41B0-A734-222D00E25BDC}", "orders_today", "Orders today", "#", MetricCategory.Orders),
                CreateSeed("{2CCD0DC7-D73C-41B0-A734-222D00E25BDC}", "revenue_today", "Income today", "$", MetricCategory.Orders),
                CreateSeed("{3CCD0DC7-D73C-41B0-A734-222D00E25BDC}", "ai_tasks_queue", "AI task in queu", "#", MetricCategory.AI),
                CreateSeed("{4CCD0DC7-D73C-41B0-A734-222D00E25BDC}", "ai_tasks_done", "AI task done", "#", MetricCategory.AI),
                CreateSeed("{5CCD0DC7-D73C-41B0-A734-222D00E25BDC}", "cpu_usage", "CPU loading", "%", MetricCategory.System)
            );
        });
    }

    private static Metric CreateSeed(string id, string key, string label, string unit, MetricCategory category)
    {
        //var id = new Guid(key.PadRight(32, '0')[..32]
          //  .Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-"));

        return Metric.CreateWithId(Guid.Parse(id), key, label, unit, category);
    }
}

