using MyMirror.TeamPulse.Domain.Entities;

namespace MyMirror.TeamPulse.Application.Interfaces;

public interface IPulseRepository
{
    Task AddAsync(PulseEntry entry, CancellationToken ct = default);
    Task<List<PulseEntry>> GetAllAsync(CancellationToken ct = default);
}
