using Microsoft.EntityFrameworkCore;
using MyMirror.TeamPulse.Domain.Entities;

namespace MyMirror.TeamPulse.Infrastructure.Persistence;

public class TeamPulseDbContext(DbContextOptions<TeamPulseDbContext> options) : DbContext(options)
{
    public DbSet<PulseEntry> PulseEntries => Set<PulseEntry>();
    public DbSet<PulseCategory> PulseCategories => Set<PulseCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PulseCategory>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<PulseEntry>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Score).IsRequired();
            e.Property(p => p.Comment).HasMaxLength(500);
            e.HasOne(p => p.Category)
             .WithMany()
             .HasForeignKey(p => p.CategoryId);
        });
    }
}
