namespace MyMirror.TeamPulse.Domain.Entities;

public class PulseCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private PulseCategory() { }

    public PulseCategory(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        Id = id;
        Name = name;
    }
}
