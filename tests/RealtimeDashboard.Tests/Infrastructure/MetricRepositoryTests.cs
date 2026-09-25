using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Infrastructure.Persistence;

namespace RealtimeDashboard.Tests.Infrastructure;

public sealed class MetricRepositoryTests
{
    private static AppDbContext CreateContext() => new(
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task GetAll_returns_seeded_metrics()
    {
        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
        var repository = new MetricRepository(context);

        var metrics = await repository.GetAllAsync();

        Assert.Equal(6, metrics.Count);
        Assert.Contains(metrics, x => x.Key == "active_users");
    }

    [Fact]
    public async Task GetByCategory_returns_only_matching_metrics()
    {
        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
        var repository = new MetricRepository(context);

        var metrics = await repository.GetByCategoryAsync(MetricCategory.AI);

        Assert.Equal(2, metrics.Count);
        Assert.All(metrics, x => Assert.Equal(MetricCategory.AI, x.Category));
    }

    [Fact]
    public async Task GetByKey_returns_metric_or_null()
    {
        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
        var repository = new MetricRepository(context);

        Assert.NotNull(await repository.GetByKeyAsync("cpu_usage"));
        Assert.Null(await repository.GetByKeyAsync("not_found"));
    }

    [Fact]
    public async Task Update_persists_changed_value()
    {
        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
        var repository = new MetricRepository(context);
        var metric = await repository.GetByKeyAsync("active_users");
        Assert.NotNull(metric);

        metric!.UpdateValue(42);
        await repository.UpdateAsync(metric);
        context.ChangeTracker.Clear();

        var persisted = await repository.GetByKeyAsync("active_users");
        Assert.Equal(42, persisted!.Value);
    }
}
