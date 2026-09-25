using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeDashboard.Application.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Key"></param>
/// <param name="Label"></param>
/// <param name="Value"></param>
/// <param name="Unit"></param>
/// <param name="Category"></param>
/// <param name="UpdatedAt"></param>
public record MetricDto(
    Guid Id,
    string Key,
    string Label,
    double Value,
    string Unit,
    string Category,
    DateTime UpdatedAt
);



