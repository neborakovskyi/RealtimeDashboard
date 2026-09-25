# Copilot instructions for RealtimeDashboard

This repository is a full-stack monitoring dashboard built with ASP.NET Core, EF Core, SignalR, and Angular. Follow the architectural boundaries and engineering conventions below.

## Project boundaries

- `RealtimeDashboard.Domain` contains the business model and domain logic.
- `RealtimeDashboard.Application` contains MediatR handlers, queries, commands, DTOs, and application abstractions.
- `RealtimeDashboard.Infrastructure` contains persistence, external clients, background jobs, and hub logic.
- `RealtimeDashboard.API` contains HTTP controllers and startup wiring.
- `realtime-dashboard-ui` contains the Angular frontend.
- `tests/RealtimeDashboard.Tests` contains xUnit tests for the backend.

Never mix domain logic into controllers or UI code. Keep infrastructure concerns in infrastructure and HTTP specifics in the API project.

## Backend guidance

- Prefer MediatR patterns for requests and operations.
- Keep controllers minimal; move logic into application handlers.
- Use EF Core migrations and model configuration in the infrastructure project.
- Use DTOs to communicate between API and clients.
- When adding AI integrations, follow the existing provider abstraction pattern based on `ILLMClient`.
- Preserve the existing `Metric` aggregate behavior when modifying domain logic.

## Testing guidance

- Add or update xUnit tests for every backend behavior change.
- Cover both successful and failure/validation paths.
- Use Moq for application-layer repository isolation.
- Use EF Core InMemory for repository tests; use a unique database name per test.
- Assert important collaborator calls, cancellation flow where relevant, and returned DTO values.
- Run `dotnet test` before considering backend work complete.
- Generate coverage when requested or when changing critical behavior:

```bash
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

Do not claim 100% coverage without checking the generated report. Startup, worker, SignalR, AI HTTP, and frontend behavior may require additional integration or component tests.

## Frontend guidance

- Preserve the Angular standalone component pattern used in this app.
- Use signals for state and computed values where appropriate.
- Prefer native template controls (`@if`, `@for`) over structural directives.
- Use `input()`, `output()`, and `model()` over older decorator-based patterns.
- Keep accessibility in scope: semantic markup, focus management, sufficient contrast, and keyboard accessibility should be considered part of the implementation.

## Realtime logic

- SignalR is a first-class pattern in this repo.
- When changing runtime updates, consider both server push and UI subscription behavior.
- The backend worker updates metrics periodically and pushes batches to clients.
- Add tests for changes to message names, payload shapes, and connection behavior where practical.

## Documentation and quality

- Update docs when changing architecture, workflows, or developer setup.
- Keep `tests/README.md` and `docs/development.md` aligned with the test commands.
- Prefer clear naming and single-responsibility components.
- Avoid broad refactors without a reason tied to the requested task.

## Validation

- Validate backend behavior with `dotnet build` and `dotnet test`.
- Validate frontend changes with `npm run test` and `npm run build` when relevant.
- Keep changes consistent with the existing project conventions.
