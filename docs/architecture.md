# Architecture overview

## High-level design

The solution follows a layered architecture with a clear separation of responsibilities:

- Domain layer: pure business concepts
- Application layer: orchestration, commands, and queries
- Infrastructure layer: persistence, external services, background jobs, and realtime stream delivery
- API layer: controller endpoints and SignalR hub exposure
- UI layer: Angular client consuming live updates

## Domain model

The central domain entity is `Metric` in `RealtimeDashboard.Domain/Entities/Metric.cs`.

```csharp
public class Metric
{
    public Guid Id { get; private set; }
    public string Key { get; private set; }
    public string Label { get; private set; }
    public double Value { get; private set; }
    public string Unit { get; private set; }
    public MetricCategory Category { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
```

The entity supports:

- creation through a factory method
- value updates with timestamp refresh
- categories such as `Users`, `Orders`, `AI`, and `System`

## Application layer

The application layer is built around MediatR and CQRS-style request handling.

Typical pattern:

- `Queries` retrieve metric data
- `Commands` issue state-changing operations or updates
- `DTOs` carry shaped data to the API and client
- interfaces abstract external dependencies

The API action in `MetricsController` dispatches queries through the mediator and returns typed DTOs to the client.

## Infrastructure layer

The infrastructure layer owns runtime concerns:

- `AppDbContext` configures SQLite and seed data
- `MetricRepository` provides persistence operations
- `MetricsWorker` simulates metric updates on a timer
- `MetricsHub` exposes the SignalR contract used by the UI
- `OllamaClient` and `OpenAIClient` implement the AI provider abstraction

### Realtime update flow

1. The background worker loads metrics from the repository.
2. It applies simulated value changes based on metric keys.
3. It persists the updated values.
4. It broadcasts the updated DTO list to all connected clients via SignalR.

## API layer

`RealtimeDashboard.API/Program.cs` contains application startup configuration:

- controller registration
- SignalR registration
- Swagger registration
- MediatR service registration
- SQLite database setup
- repository registration
- AI provider configuration via `AI:Provider`
- CORS policy for Angular
- SignalR hub mapping at `/hubs/metrics`

## UI layer

The Angular frontend in `realtime-dashboard-ui` is responsible for displaying live dashboard metrics. It expects the backend API and SignalR hub to be running locally and subscribes to metric streams for display and category-based views.

The repo also contains a root-level frontend rule file in `realtime-dashboard-ui/AGENTS.md`, which captures Angular-specific engineering expectations for Copilot and contributors.

## Design principles

- Keep business behaviors inside the domain model
- Keep orchestration in the application layer
- Keep infrastructure concerns separate from domain logic
- Expose real-time updates through SignalR rather than polling the UI
- Use abstraction for AI providers instead of hard-coding a single service

## Consequences

This structure makes it easier to:

- add new metrics with minimal changes
- swap AI providers without touching API behavior
- extend the UI without forcing backend changes
- test application flows through MediatR-based handlers
- scale the project into a more production-oriented monitoring service
