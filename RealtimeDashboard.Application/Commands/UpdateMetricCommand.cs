using MediatR;
using RealtimeDashboard.Application.DTOs;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Commands;

public record UpdateMetricCommand(string Key, double Value) : IRequest<MetricDto?>;

public class UpdateMetricCommandHandler(IMetricRepository repository) : IRequestHandler<UpdateMetricCommand, MetricDto?>
{
    public async Task<MetricDto?> Handle(UpdateMetricCommand request, CancellationToken ct)
    {
        var metric = await repository.GetByKeyAsync(request.Key, ct);
        if (metric is null) return null;

        metric.UpdateValue(request.Value);
        await repository.UpdateAsync(metric, ct);

        return new MetricDto(
            metric.Id,
            metric.Key,
            metric.Label,
            metric.Value,
            metric.Unit,
            metric.Category.ToString(),
            metric.UpdatedAt);
    }

}

