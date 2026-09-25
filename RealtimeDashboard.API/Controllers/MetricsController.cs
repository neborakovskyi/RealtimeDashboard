using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealtimeDashboard.Application.DTOs;
using RealtimeDashboard.Application.Queries;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetricsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MetricDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMetricsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IReadOnlyList<MetricDto>>> GetByCategory(string category, CancellationToken ct)
    {
        if (!Enum.TryParse<MetricCategory>(category, true, out var cat))
        {
            return BadRequest($"Unknown category: {category}");
        }

        var result = await mediator.Send(new GetMetricsQuery(cat), ct);
        return Ok(result);
    }
}
