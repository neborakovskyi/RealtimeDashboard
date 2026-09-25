# RealtimeDashboard

RealtimeDashboard is a full-stack monitoring sample that combines a .NET backend, EF Core persistence, SignalR real-time updates, and an Angular dashboard UI. The solution is designed to demonstrate live metric streaming, category-based filtering, and AI-ready analysis hooks.

## Solution overview

- Backend: ASP.NET Core API in `RealtimeDashboard.API`
- Application layer: CQRS + MediatR in `RealtimeDashboard.Application`
- Domain model: core entities and abstractions in `RealtimeDashboard.Domain`
- Infrastructure: persistence, SignalR hub, background worker, and AI clients in `RealtimeDashboard.Infrastructure`
- Frontend: Angular app in `realtime-dashboard-ui`

## Architecture summary

- `RealtimeDashboard.API`
  - Registers controllers, SignalR, Swagger, and background services
  - Exposes metric endpoints and real-time hub endpoints
- `RealtimeDashboard.Application`
  - Contains commands, queries, DTOs, and interfaces used by the API layer
  - Uses MediatR for request processing
- `RealtimeDashboard.Domain`
  - Holds the `Metric` aggregate and `MetricCategory` enum
  - Defines the domain contracts used by the infrastructure and application layers
- `RealtimeDashboard.Infrastructure`
  - Implements EF Core SQLite persistence, repository access, hub logic, and worker-based metric updates
  - Contains AI clients for Ollama/OpenAI strategy selection
- `realtime-dashboard-ui`
  - Angular client subscribed to live updates via SignalR
  - Uses Angular Material and modern standalone component patterns

## Core capabilities

- Real-time metric updates every few seconds
- SignalR push to connected Angular clients
- Metric queries by all metrics or category
- SQLite-backed persistence with initial seed data
- Pluggable AI provider abstraction using OpenAI or Ollama

## Repository structure

```text
RealtimeDashboard/
├── RealtimeDashboard.API/
│   ├── Controllers/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── RealtimeDashboard.API.csproj
├── RealtimeDashboard.Application/
│   ├── Commands/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Queries/
│   └── RealtimeDashboard.Application.csproj
├── RealtimeDashboard.Domain/
│   ├── Entities/
│   ├── Events/
│   ├── Interfaces/
│   └── RealtimeDashboard.Domain.csproj
├── RealtimeDashboard.Infrastructure/
│   ├── Hubs/
│   ├── Migrations/
│   ├── Persistence/
│   ├── Services/
│   └── RealtimeDashboard.Infrastructure.csproj
├── realtime-dashboard-ui/
│   ├── src/
│   ├── package.json
│   ├── angular.json
│   └── README.md
├── RealtimeDashboard.slnx
├── .gitignore
├── .gitattributes
└── README.md
```

## Run the solution

### 1. Backend

```bash
dotnet restore
cd RealtimeDashboard.API
dotnet run
```

The API will start with Swagger enabled in Development mode and expose SignalR on `/hubs/metrics`.

### 2. Frontend

```bash
cd realtime-dashboard-ui
npm install
npm start
```

The Angular UI connects to `http://localhost:4200` and expects the API to be available locally.

## Key technologies

- .NET 10
- ASP.NET Core Web API
- MediatR
- Entity Framework Core + SQLite
- ASP.NET Core SignalR
- Angular 22
- TypeScript
- Angular Material
- OpenAI / Ollama client abstraction

## Documentation index

- `docs/architecture.md` — architecture and project design decisions
- `docs/development.md` — developer workflow and troubleshooting
- `.github/copilot-instructions.md` — repo-wide AI coding guidance
- `.github/instructions/*.instructions.md` — domain-specific instructions for backend and frontend work
- `.github/skills/*.skill.md` — reusable solution-aware development skills

## Notes

This repository is intended as a working sample and a template for future real-time monitoring solutions. Keep domain boundaries clean: domain logic lives in `RealtimeDashboard.Domain`, orchestration in `Application`, infrastructure in `Infrastructure`, and external exposure in `API`.
