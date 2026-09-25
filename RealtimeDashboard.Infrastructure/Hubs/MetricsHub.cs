using Microsoft.AspNetCore.SignalR;
using RealtimeDashboard.Application.DTOs;

namespace RealtimeDashboard.Infrastructure.Hubs;

public class MetricsHub : Hub
{
    public async Task SubscribeToCategory(string category)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, category);
    }
    public async Task UnsubscribeToCategory(string category)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, category);
    }
}

public interface IMetricsHubClient
{
    Task MetricsUpdate(MetricDto metric);
    Task MetricsBatchUpdated(IReadOnlyList<MetricDto> metrics);
}
