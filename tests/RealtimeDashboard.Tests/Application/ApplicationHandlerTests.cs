using Moq;
using RealtimeDashboard.Application.Commands;
using RealtimeDashboard.Application.Queries;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Tests.Application;

public sealed class ApplicationHandlerTests
{
    [Fact]
    public async Task GetMetrics_without_category_uses_get_all_and_maps_dto()
    {
        var metric = Metric.Create("active_users", "Active users", "#", MetricCategory.Users);
        metric.UpdateValue(10);
        var repository = new Mock<IMetricRepository>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { metric });
        var handler = new GetMetricsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetMetricsQuery(), CancellationToken.None);

        var dto = Assert.Single(result);
        Assert.Equal(metric.Id, dto.Id);
        Assert.Equal("Users", dto.Category);
        Assert.Equal(10, dto.Value);
        repository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.GetByCategoryAsync(It.IsAny<MetricCategory>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetMetrics_with_category_uses_filtered_repository_method()
    {
        var repository = new Mock<IMetricRepository>();
        repository.Setup(x => x.GetByCategoryAsync(MetricCategory.AI, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Metric>());
        var handler = new GetMetricsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetMetricsQuery(MetricCategory.AI), CancellationToken.None);

        Assert.Empty(result);
        repository.Verify(x => x.GetByCategoryAsync(MetricCategory.AI, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMetric_returns_null_when_key_does_not_exist()
    {
        var repository = new Mock<IMetricRepository>();
        repository.Setup(x => x.GetByKeyAsync("missing", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Metric?)null);
        var handler = new UpdateMetricCommandHandler(repository.Object);

        var result = await handler.Handle(new UpdateMetricCommand("missing", 1), CancellationToken.None);

        Assert.Null(result);
        repository.Verify(x => x.UpdateAsync(It.IsAny<Metric>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateMetric_updates_metric_and_returns_dto()
    {
        var metric = Metric.Create("cpu_usage", "CPU", "%", MetricCategory.System);
        var repository = new Mock<IMetricRepository>();
        repository.Setup(x => x.GetByKeyAsync("cpu_usage", It.IsAny<CancellationToken>()))
            .ReturnsAsync(metric);
        var handler = new UpdateMetricCommandHandler(repository.Object);

        var result = await handler.Handle(new UpdateMetricCommand("cpu_usage", 81.25), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(81.25, result!.Value);
        Assert.Equal("System", result.Category);
        repository.Verify(x => x.UpdateAsync(metric, It.IsAny<CancellationToken>()), Times.Once);
    }
}
