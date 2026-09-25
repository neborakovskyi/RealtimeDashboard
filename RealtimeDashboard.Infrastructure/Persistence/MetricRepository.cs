using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Infrastructure.Persistence
{
    public class MetricRepository(AppDbContext db) : IMetricRepository
    {
        public async Task<IReadOnlyList<Metric>> GetAllAsync(CancellationToken cancellationToken = default) => await db.Metrics.AsNoTracking().ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Metric>> GetByCategoryAsync(MetricCategory category, CancellationToken cancellationToken = default) => await db.Metrics.AsNoTracking().Where(w => w.Category == category).ToListAsync(cancellationToken);

        public async Task<Metric?> GetByKeyAsync(string key, CancellationToken cancellationToken = default) => await db.Metrics.FirstOrDefaultAsync(w => w.Key == key, cancellationToken);

        public async Task UpdateAsync(Metric metric, CancellationToken cancellationToken = default)
        {
            db.Metrics.Update(metric);
            await db.SaveChangesAsync(cancellationToken);
        }
        
    }
}
