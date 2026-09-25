using Microsoft.AspNetCore.Mvc;
using Moq;
using RealtimeDashboard.API.Controllers;
using RealtimeDashboard.Application.DTOs;
using RealtimeDashboard.Application.Queries;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Tests.API;

public sealed class MetricsControllerTests
{
    [Fact]
    public async Task GetAll_returns_ok_with_metrics()
    {
        var mediator = new Mock<MediatR.IMediator>();
        var expected = new[] { new MetricDto(Guid.NewGuid(), "active_users", "Active users", 3, "#", "Users", DateTime.UtcNow) };
        mediator.Setup(x => x.Send(It.IsAny<GetMetricsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var controller = new MetricsController(mediator.Object);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
    }

    [Fact]
    public async Task GetByCategory_returns_bad_request_for_unknown_category()
    {
        var controller = new MetricsController(Mock.Of<MediatR.IMediator>());

        var result = await controller.GetByCategory("unknown", CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Unknown category", badRequest.Value!.ToString());
    }

    [Fact]
    public async Task GetByCategory_returns_ok_for_valid_category()
    {
        var mediator = new Mock<MediatR.IMediator>();
        mediator.Setup(x => x.Send(It.Is<GetMetricsQuery>(q => q.Category == MetricCategory.Orders), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<MetricDto>());
        var controller = new MetricsController(mediator.Object);

        var result = await controller.GetByCategory("orders", CancellationToken.None);

        Assert.IsType<OkObjectResult>(result.Result);
        mediator.Verify(x => x.Send(It.Is<GetMetricsQuery>(q => q.Category == MetricCategory.Orders), It.IsAny<CancellationToken>()), Times.Once);
    }
}
