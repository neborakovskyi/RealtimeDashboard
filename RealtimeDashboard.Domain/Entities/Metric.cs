namespace RealtimeDashboard.Domain.Entities;

/// <summary>
/// Represents a metric entity with properties such as Id, Key, Label, Value, Unit, Category, and UpdatedAt.
/// </summary>
public class Metric
{
    public Guid Id { get; private set; } // Unique identifier for the metric
    public string Key { get; private set; }// Unique key for the metric
    public string Label { get; private set; }// Display label for the metric
    public double Value { get; private set; }// Current value of the metric
    public string Unit { get; private set; }// Unit of measurement for the metric
    public MetricCategory Category { get; private set; }// Category of the metric (e.g., Users, Orders, AI, System)
    public DateTime UpdatedAt { get; private set; }// Timestamp indicating when the metric was last updated

    // Private constructor to prevent direct instantiation
    private Metric() { Key = Label = Unit = string.Empty; }
    // Factory method to create a new Metric instance with the specified properties
    public static Metric Create(string key, string label, string unit, MetricCategory category)
    {
        return new Metric
        {
            Id = Guid.NewGuid(),
            Key = key,
            Label = label,
            Value = 0,
            Unit = unit,
            Category = category,
            UpdatedAt = DateTime.UtcNow
        };
    }
    // Method to update the value of the metric and set the UpdatedAt timestamp to the current UTC time
    public void UpdateValue(double value)
    {
        Value = value;
        UpdatedAt = DateTime.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="key"></param>
    /// <param name="label"></param>
    /// <param name="unit"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    public static Metric CreateWithId(Guid id, string key, string label, string unit, MetricCategory category)
    {
        return new Metric
        {
            Id = id,
            Key = key,
            Label = label,
            Value = 0,
            Unit = unit,
            Category = category,
            UpdatedAt = DateTime.MinValue
        };
    }
}
/// <summary>
/// Enumeration representing the different categories of metrics, including Users, Orders, AI, and System.
/// </summary>
public enum MetricCategory
{
    Users = 1,
    Orders,
    AI,
    System
}