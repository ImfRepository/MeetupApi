using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Meetup.Infrastructure.SQL;

public class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> opt) : base(opt)
    { }

    public DbSet<MeetupEntity> Events { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<PlanStep> PlanSteps { get; set; }
    public DbSet<Organizer> Organizers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeetupEntity>()
            .ToTable("events")
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<PlanStep>()
            .ToTable("plan_steps")
            .HasOne(e => e.Meetup)
            .WithMany(e => e.PlanSteps)
            .HasForeignKey(e => e.EventId)
            .HasPrincipalKey(e => e.Id);

        modelBuilder.Entity<Place>()
            .ToTable("places")
            .HasMany(e => e.Events)
            .WithOne(e => e.Place)
            .HasForeignKey(e => e.PlaceId)
            .HasPrincipalKey(e => e.Id);

        modelBuilder.Entity<Organizer>()
            .ToTable("organizers")
            .HasMany(e => e.Events)
            .WithOne(e => e.Organizer)
            .HasForeignKey(e => e.OrganizerId)
            .HasPrincipalKey(e => e.Id);
    }
}