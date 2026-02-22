using MediatR;
using MyMirror.TeamPulse.Application.Interfaces;
using MyMirror.TeamPulse.Domain.Entities;

namespace MyMirror.TeamPulse.Application.Features;

public record SubmitPulseCommand : IRequest<Guid>
{
    public int Score { get; init; }
    public string? Comment { get; init; }
    public Guid CategoryId { get; init; }
}

public class SubmitPulseHandler(IPulseRepository pulseRepo, ICategoryRepository categoryRepo) : IRequestHandler<SubmitPulseCommand, Guid>
{
    private readonly IPulseRepository _pulseRepo = pulseRepo;
    private readonly ICategoryRepository _categoryRepo = categoryRepo;

    public async Task<Guid> Handle(SubmitPulseCommand request, CancellationToken ct)
    {
        if (!await _categoryRepo.ExistsAsync(request.CategoryId, ct))
            throw new ArgumentException($"Category '{request.CategoryId}' does not exist.");

        var entry = new PulseEntry(request.Score, request.Comment, request.CategoryId);
        await _pulseRepo.AddAsync(entry, ct);
        return entry.Id;
    }
}
