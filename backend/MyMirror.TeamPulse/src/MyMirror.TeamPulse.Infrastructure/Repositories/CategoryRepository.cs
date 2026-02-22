using Microsoft.EntityFrameworkCore;
using MyMirror.TeamPulse.Application.Interfaces;
using MyMirror.TeamPulse.Domain.Entities;
using MyMirror.TeamPulse.Infrastructure.Persistence;

namespace MyMirror.TeamPulse.Infrastructure.Repositories;

public class CategoryRepository(TeamPulseDbContext db) : ICategoryRepository
{
    private readonly TeamPulseDbContext _db = db;

    public async Task<List<PulseCategory>> GetAllAsync(CancellationToken ct = default)
        => await _db.PulseCategories.AsNoTracking().ToListAsync(ct);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await _db.PulseCategories.AnyAsync(c => c.Id == id, ct);
}
