using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealtimeDashboard.Application.DTOs;
using RealtimeDashboard.Domain.Interfaces;
using RealtimeDashboard.Infrastructure.Hubs;

namespace RealtimeDashboard.Infrastructure.Services;

public class MetricsWorker(IServiceScopeFactory scopeFactory, IHubContext<MetricsHub> hub, ILogger<MetricsWorker> logger) : BackgroundService
{
    private readonly Random _random = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("MetricsWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateMetricsAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MetricsWorker");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        logger.LogInformation("MetricsWorker stopped");
    }

    private async Task UpdateMetricsAsync(CancellationToken stoppingToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IMetricRepository>();

        var metrics = await repository.GetAllAsync(stoppingToken);
        var updatedDtos = new List<MetricDto>();

        foreach (var metric in metrics)
        {
            var newValue = metric.Key switch
            {
                "active_users" => Math.Max(0, metric.Value + _random.Next(-5, 10)),
                "orders_today" => metric.Value + _random.Next(0, 3),
                "revenue_today" => metric.Value + _random.Next(0, 500),
                "ai_tasks_queue" => Math.Max(0, metric.Value + _random.Next(-2, 5)),
                "ai_tasks_done" => metric.Value + _random.Next(0, 2),
                "cpu_usage" => Math.Clamp(metric.Value + _random.Next(-5, 5), 0, 100),
                _ => metric.Value
            };

            metric.UpdateValue(newValue);
            await repository.UpdateAsync(metric, stoppingToken);

            updatedDtos.Add(new MetricDto(
                metric.Id,
                metric.Key,
                metric.Label,
                metric.Value,
                metric.Unit,
                metric.Category.ToString(),
                metric.UpdatedAt));
        }

        await hub.Clients.All.SendAsync("MetricsBatchUpdated", updatedDtos, stoppingToken);

        logger.LogDebug("Pushed {Count} metrics to clients", updatedDtos.Count);
    }
}
