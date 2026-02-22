using MyMirror.TeamPulse.Domain.Entities;

namespace MyMirror.TeamPulse.Tests.Domain;

public class PulseEntryTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Constructor_InvalidScore_ThrowsArgumentOutOfRangeException(int score)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PulseEntry(score, null, Guid.NewGuid()));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Constructor_ValidScore_CreatesEntry(int score)
    {
        var categoryId = Guid.NewGuid();
        var entry = new PulseEntry(score, "feeling good", categoryId);

        Assert.Equal(score, entry.Score);
        Assert.Equal("feeling good", entry.Comment);
        Assert.Equal(categoryId, entry.CategoryId);
    }

    [Fact]
    public void Constructor_CommentTooLong_ThrowsArgumentException()
    {
        var longComment = new string('x', PulseEntry.MaxCommentLength + 1);

        Assert.Throws<ArgumentException>(() =>
            new PulseEntry(3, longComment, Guid.NewGuid()));
    }

    [Fact]
    public void Constructor_NullComment_IsAllowed()
    {
        var entry = new PulseEntry(3, null, Guid.NewGuid());

        Assert.Null(entry.Comment);
    }

    [Fact]
    public void Constructor_AssignsNewId()
    {
        var entry1 = new PulseEntry(3, null, Guid.NewGuid());
        var entry2 = new PulseEntry(3, null, Guid.NewGuid());

        Assert.NotEqual(entry1.Id, entry2.Id);
    }
}
