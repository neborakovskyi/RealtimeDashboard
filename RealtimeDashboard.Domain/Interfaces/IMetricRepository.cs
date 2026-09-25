using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Domain.Interfaces;

public interface IMetricRepository
{
    Task<IReadOnlyList<Metric>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Metric>> GetByCategoryAsync(MetricCategory category, CancellationToken cancellationToken = default);
    Task<Metric?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task UpdateAsync(Metric metric, CancellationToken cancellationToken = default);
}
