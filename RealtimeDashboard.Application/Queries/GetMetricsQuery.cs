using MediatR;
using RealtimeDashboard.Application.DTOs;
using RealtimeDashboard.Domain.Interfaces;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Application.Queries;


public record GetMetricsQuery(MetricCategory? Category = null) : IRequest<IReadOnlyList<MetricDto>>;

public class GetMetricsQueryHandler(IMetricRepository repository) : IRequestHandler<GetMetricsQuery, IReadOnlyList<MetricDto>>
{
    public async Task<IReadOnlyList<MetricDto>> Handle(GetMetricsQuery request, CancellationToken ct)
    {
        var metrics = request.Category.HasValue
            ? await repository.GetByCategoryAsync(request.Category.Value, ct)
            : await repository.GetAllAsync(ct);


        return [.. metrics
            .Select(m => new MetricDto(
                m.Id,
                m.Key,
                m.Label,
                m.Value,
                m.Unit,
                m.Category.ToString(),
                m.UpdatedAt))];
    }
}