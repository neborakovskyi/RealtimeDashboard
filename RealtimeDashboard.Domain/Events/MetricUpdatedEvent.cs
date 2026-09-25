using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Domain.Events;

/// <summary>
/// Represents an event that is triggered when a metric is updated, containing properties such as MetricId, Key, Label, Value, Unit, Category, and UpdatedAt.
/// </summary>
/// <param name="MetricId"></param>
/// <param name="Key"></param>
/// <param name="Label"></param>
/// <param name="Value"></param>
/// <param name="Unit"></param>
/// <param name="Category"></param>
/// <param name="UpdatedAt"></param>
public record MetricUpdatedEvent(
    Guid MetricId,
    string Key,
    string Label,
    double Value,
    string Unit,
    MetricCategory Category,
    DateTime UpdatedAt
)
{
    
}
