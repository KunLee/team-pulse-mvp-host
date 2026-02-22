namespace MyMirror.TeamPulse.Domain.Entities;

public class PulseEntry
{
    public const int MinScore = 1;
    public const int MaxScore = 5;
    public const int MaxCommentLength = 500;

    public Guid Id { get; private set; }
    public int Score { get; private set; }
    public string? Comment { get; private set; }
    public Guid CategoryId { get; private set; }
    public PulseCategory Category { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private PulseEntry() { }

    public PulseEntry(int score, string? comment, Guid categoryId)
    {
        if (score < MinScore || score > MaxScore)
            throw new ArgumentOutOfRangeException(nameof(score), $"Score must be between {MinScore} and {MaxScore}.");

        if (comment is not null && comment.Length > MaxCommentLength)
            throw new ArgumentException($"Comment must not exceed {MaxCommentLength} characters.", nameof(comment));

        Id = Guid.NewGuid();
        Score = score;
        Comment = comment;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }
}
