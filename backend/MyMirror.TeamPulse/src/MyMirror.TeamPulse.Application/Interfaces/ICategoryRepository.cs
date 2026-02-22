using MyMirror.TeamPulse.Domain.Entities;

namespace MyMirror.TeamPulse.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<PulseCategory>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}
