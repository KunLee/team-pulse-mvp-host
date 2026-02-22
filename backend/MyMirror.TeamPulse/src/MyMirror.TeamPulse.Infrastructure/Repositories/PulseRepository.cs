using Microsoft.EntityFrameworkCore;
using MyMirror.TeamPulse.Application.Interfaces;
using MyMirror.TeamPulse.Domain.Entities;
using MyMirror.TeamPulse.Infrastructure.Persistence;

namespace MyMirror.TeamPulse.Infrastructure.Repositories;

public class PulseRepository(TeamPulseDbContext db) : IPulseRepository
{
    private readonly TeamPulseDbContext _db = db;

    public async Task AddAsync(PulseEntry entry, CancellationToken ct = default)
    {
        _db.PulseEntries.Add(entry);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<List<PulseEntry>> GetAllAsync(CancellationToken ct = default)
        => await _db.PulseEntries.AsNoTracking().ToListAsync(ct);
}
