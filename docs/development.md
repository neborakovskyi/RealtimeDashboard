# Development guide

## Local setup

### Prerequisites

- .NET 10 SDK
- Node.js 20+
- npm
- Optional: Ollama or OpenAI access for AI features

### Backend

```bash
dotnet restore
cd RealtimeDashboard.API
dotnet run
```

In development mode, Swagger is enabled and migrations are applied automatically.

### Frontend

```bash
cd realtime-dashboard-ui
npm install
npm start
```

The Angular app runs on `http://localhost:4200` by default.

## Typical workflows

### Add a new metric

1. Update the metric seed data in `AppDbContext` if you want it available from startup.
2. Extend the simulation logic in `MetricsWorker` if the metric should vary over time.
3. Ensure the category is part of `MetricCategory`.
4. Update the UI model and dashboard rendering if needed.

### Add an API endpoint

1. Add a query or command in `RealtimeDashboard.Application`.
2. Register the handler with MediatR conventions.
3. Expose the endpoint in a controller under `RealtimeDashboard.API/Controllers`.
4. Return DTOs instead of domain entities where possible.

### Add a frontend metric card

1. Inspect the existing Angular dashboard structure.
2. Reuse the SignalR service pattern already used by the app.
3. Keep state local and signal-based following the existing frontend guidance.
4. Prefer accessible markup and material components.

## Troubleshooting

### API fails to start

- Confirm the .NET SDK is installed.
- Verify the `AI:Provider` and related settings in `appsettings.json`.
- Ensure SQLite database files are writable in the API project.

### Frontend cannot connect to backend

- Confirm the API is running on the expected local port.
- Check the Angular CORS configuration in `Program.cs`.
- Ensure the SignalR hub URL matches the UI expectations.

### AI client errors

- If using Ollama, verify the server is listening at the configured URL, usually `http://localhost:11434/`.
- If using OpenAI, verify the API key is valid and the provider is configured correctly.

## Coding conventions

### Backend conventions

- Keep domain logic in `RealtimeDashboard.Domain`.
- Prefer interfaces and abstractions in the application layer.
- Use MediatR for request routing.
- Keep controllers thin and focused on HTTP concerns.

### Frontend conventions

- Prefer standalone Angular components.
- Use signals for local state.
- Avoid `ngClass` and `ngStyle` in favor of binding syntax.
- Keep components focused and small.
- Respect accessibility requirements and AXE compliance.

## Useful commands

```bash
# Restore .NET dependencies
dotnet restore

# Build the .NET solution
dotnet build

# Run backend
dotnet run --project RealtimeDashboard.API

# Run frontend
cd realtime-dashboard-ui && npm install && npm start
```
