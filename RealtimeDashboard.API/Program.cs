using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Application.Commands;
using RealtimeDashboard.Application.Interfaces;
using RealtimeDashboard.Domain.Interfaces;
using RealtimeDashboard.Infrastructure.Hubs;
using RealtimeDashboard.Infrastructure.Persistence;
using RealtimeDashboard.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers + SignalR ──────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── MediatR — сканирует handlers автоматически ────────────────
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateMetricCommand).Assembly));

// ── EF Core + SQLite ──────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=metrics.db"));

// ── Repository ────────────────────────────────────────────────
builder.Services.AddScoped<IMetricRepository, MetricRepository>();

// ── AI клиент — выбор через конфиг (Strategy паттерн) ────────
var aiProvider = builder.Configuration["AI:Provider"] ?? "Ollama";

if (aiProvider == "OpenAI")
{
    builder.Services.AddHttpClient<ILLMClient, OpenAIClient>(http =>
    {
        http.BaseAddress = new Uri("https://api.openai.com/v1/");
        http.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["AI:ApiKey"]}");
    });
}
else
{
    builder.Services.AddHttpClient<ILLMClient, OllamaClient>(http =>
    {
        http.BaseAddress = new Uri(builder.Configuration["AI:OllamaUrl"] ?? "http://localhost:11434/");
    });
}

// ── Background Worker ─────────────────────────────────────────
builder.Services.AddHostedService<MetricsWorker>();

// ── CORS для Angular ──────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials())); // нужно для SignalR

// ─────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Миграции при старте (Dev only) ───────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");
app.UseAuthorization();
app.MapControllers();

// SignalR endpoint
app.MapHub<MetricsHub>("/hubs/metrics");

app.Run();