namespace MyMirror.TeamPulse.Application.DTOs;

public record PulseSummaryDto(
    int Count,
    double AverageScore,
    Dictionary<int, int> Scores,
    List<CategorySummaryDto> Categories
);

public record CategorySummaryDto(Guid Id, string Name, int Count);
