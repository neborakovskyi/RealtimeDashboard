# RealtimeDashboard.Tests

xUnit tests for the .NET solution.

## Run tests

```bash
dotnet test
```

## Generate coverage

The test project includes Coverlet's data collector. Generate an OpenCover report with:

```bash
dotnet test tests/RealtimeDashboard.Tests/RealtimeDashboard.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

Coverage files are written beneath `TestResults`. To inspect the report locally, install a report generator and run:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator \
  -reports:"TestResults/**/coverage.cobertura.xml" \
  -targetdir:"TestResults/report" \
  -reporttypes:Html
```

The tests cover the domain entity, MediatR handlers, repository operations, seeded persistence, and controller branches. Exact 100% coverage depends on the current implementation and should be verified with the generated report; coverage targets should not be achieved by excluding untested production code.
