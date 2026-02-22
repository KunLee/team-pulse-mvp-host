using MyMirror.TeamPulse.Domain.Entities;

namespace MyMirror.TeamPulse.Infrastructure.Persistence;

public static class DataSeeder
{
    public static void Seed(TeamPulseDbContext db)
    {
        if (db.PulseCategories.Any()) return;

        var categories = new[]
        {
            new PulseCategory(Guid.NewGuid(), "Workload"),
            new PulseCategory(Guid.NewGuid(), "Collaboration"),
            new PulseCategory(Guid.NewGuid(), "Wellbeing"),
            new PulseCategory(Guid.NewGuid(), "Team Dynamics"),
            new PulseCategory(Guid.NewGuid(), "Environment"),
        };

        db.PulseCategories.AddRange(categories);
        db.SaveChanges();
    }
}
