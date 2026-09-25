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

## Testing

### Run all .NET tests

Run from the repository root:

```bash
dotnet test
```

The test project is `tests/RealtimeDashboard.Tests` and uses xUnit, Moq, EF Core InMemory, and Coverlet.

### Run only the backend test project

```bash
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj
```

### Run a selected test

```bash
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj \
  --filter "FullyQualifiedName~MetricTests"
```

### Generate code coverage

```bash
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

Coverlet writes the coverage result under `TestResults`. Generate a readable HTML report with ReportGenerator:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator \
  -reports:"TestResults/**/coverage.cobertura.xml" \
  -targetdir:"TestResults/report" \
  -reporttypes:Html
```

Open `TestResults/report/index.html` after report generation. Exact 100% is a measurable result, not an assumption; use the report to identify uncovered lines and branches.

### Test scope

The current tests cover:

- domain entity factories and value updates
- application query and command handlers
- repository reads, filtering, lookup, and updates
- EF Core seed data
- controller success and invalid-input branches

Additional integration tests are needed for full solution coverage of startup registration, `MetricsWorker`, SignalR hub interactions, OpenAI/Ollama HTTP clients, and Angular behavior.

## Typical workflows

### Add a new metric

1. Update the metric seed data in `AppDbContext` if you want it available from startup.
2. Extend the simulation logic in `MetricsWorker` if the metric should vary over time.
3. Ensure the category is part of `MetricCategory`.
4. Add or update domain, application, repository, controller, and UI tests as applicable.
5. Update the UI model and dashboard rendering if needed.
6. Run `dotnet test` and generate coverage for backend changes.

### Add an API endpoint

1. Add a query or command in `RealtimeDashboard.Application`.
2. Register the handler with MediatR conventions.
3. Expose the endpoint in a controller under `RealtimeDashboard.API/Controllers`.
4. Return DTOs instead of domain entities where possible.
5. Add handler tests and controller tests for valid and invalid paths.
6. Run the backend test suite.

### Add a frontend metric card

1. Inspect the existing Angular dashboard structure.
2. Reuse the SignalR service pattern already used by the app.
3. Keep state local and signal-based following the existing frontend guidance.
4. Prefer accessible markup and material components.
5. Add or update Angular unit tests where the component behavior changes.
6. Run `npm test` and `npm run build`.

## Troubleshooting

### API fails to start

- Confirm the .NET SDK is installed.
- Verify the `AI:Provider` and related settings in `appsettings.json`.
- Ensure SQLite database files are writable in the API project.

### Tests fail during database setup

- Confirm the test project restores `Microsoft.EntityFrameworkCore.InMemory`.
- Ensure each test uses an isolated in-memory database name.
- Run the test project directly to distinguish test failures from solution build failures.

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
- Pass `CancellationToken` through asynchronous operations.
- Add tests for new branches and behavior, not only happy paths.

### Frontend conventions

- Prefer standalone Angular components.
- Use signals for local state.
- Avoid `ngClass` and `ngStyle` in favor of binding syntax.
- Keep components focused and small.
- Respect accessibility requirements and AXE compliance.
- Run frontend tests and builds for UI changes.

## Useful commands

```bash
# Restore .NET dependencies
dotnet restore

# Build the .NET solution
dotnet build

# Run all .NET tests
dotnet test

# Run backend tests only
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj

# Generate coverage
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Run backend
dotnet run --project RealtimeDashboard.API

# Run frontend
cd realtime-dashboard-ui && npm install && npm start

# Run frontend tests
cd realtime-dashboard-ui && npm test

# Build frontend
cd realtime-dashboard-ui && npm run build
```
