using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Tests.Domain;

public sealed class MetricTests
{
    [Fact]
    public void Create_initializes_metric_with_defaults()
    {
        var metric = Metric.Create("orders_today", "Orders today", "#", MetricCategory.Orders);

        Assert.NotEqual(Guid.Empty, metric.Id);
        Assert.Equal("orders_today", metric.Key);
        Assert.Equal("Orders today", metric.Label);
        Assert.Equal("#", metric.Unit);
        Assert.Equal(MetricCategory.Orders, metric.Category);
        Assert.Equal(0, metric.Value);
        Assert.True(metric.UpdatedAt > DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void UpdateValue_changes_value_and_timestamp()
    {
        var metric = Metric.Create("cpu_usage", "CPU", "%", MetricCategory.System);
        var before = metric.UpdatedAt;

        metric.UpdateValue(72.5);

        Assert.Equal(72.5, metric.Value);
        Assert.True(metric.UpdatedAt >= before);
    }

    [Fact]
    public void CreateWithId_preserves_id_and_uses_minimum_timestamp()
    {
        var id = Guid.NewGuid();

        var metric = Metric.CreateWithId(id, "active_users", "Active users", "#", MetricCategory.Users);

        Assert.Equal(id, metric.Id);
        Assert.Equal(DateTime.MinValue, metric.UpdatedAt);
    }
}
