# Backend instructions

Apply these instructions when working in the ASP.NET Core, EF Core, and infrastructure portions of this repository.

## Architecture

- Keep `RealtimeDashboard.Domain` free of infrastructure dependencies.
- Keep `RealtimeDashboard.Application` focused on use cases, DTOs, and orchestration.
- Keep `RealtimeDashboard.Infrastructure` responsible for persistence, I/O, and external integrations.
- Keep `RealtimeDashboard.API` responsible only for HTTP contracts and startup composition.

## Coding standards

- Use nullable reference types and modern C# conventions.
- Favor explicit, small, readable models over over-engineered abstractions.
- Use MediatR and type-safe queries/commands consistently.
- Prefer `async`/`await` patterns and `CancellationToken` propagation.

## Persistence and runtime

- Update SQLite seed data carefully when changing metric defaults.
- Keep metric keys stable and predictable so UI and worker logic remain aligned.
- Consider the impact of background jobs on server load and client session behavior.

## AI integration

- Prefer the existing provider strategy pattern rather than introducing hard-coded AI provider logic in controllers or services.
- Keep OpenAI and Ollama-specific details inside the infrastructure service implementations.
