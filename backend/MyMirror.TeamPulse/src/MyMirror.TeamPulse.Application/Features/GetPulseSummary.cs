using MediatR;
using MyMirror.TeamPulse.Application.DTOs;
using MyMirror.TeamPulse.Application.Interfaces;

namespace MyMirror.TeamPulse.Application.Features;

public record GetPulseSummaryQuery : IRequest<PulseSummaryDto>;

public class GetPulseSummaryHandler(IPulseRepository pulseRepo, ICategoryRepository categoryRepo) : IRequestHandler<GetPulseSummaryQuery, PulseSummaryDto>
{
    private readonly IPulseRepository _pulseRepo = pulseRepo;
    private readonly ICategoryRepository _categoryRepo = categoryRepo;

    public async Task<PulseSummaryDto> Handle(GetPulseSummaryQuery request, CancellationToken ct)
    {
        var entries = await _pulseRepo.GetAllAsync(ct);
        var categories = await _categoryRepo.GetAllAsync(ct);

        var count = entries.Count;
        var average = count > 0 ? Math.Round(entries.Average(e => e.Score), 1) : 0.0;
        var scores = Enumerable.Range(1, 5).ToDictionary(s => s, s => entries.Count(e => e.Score == s));
        var categorySummaries = categories
            .Select(c => new CategorySummaryDto(c.Id, c.Name, entries.Count(e => e.CategoryId == c.Id)))
            .ToList();

        return new PulseSummaryDto(count, average, scores, categorySummaries);
    }
}
