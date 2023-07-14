using Meetup.Infrastructure.Data.Entities;
using Meetup.Infrastructure.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

internal class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> opt) : base(opt)
    { }

    public DbSet<MeetupEntity> Meetups { get; set; }
    public DbSet<PlaceEntity> Places { get; set; }
    public DbSet<PlanStepEntity> PlanSteps { get; set; }
    public DbSet<OrganizerEntity> Organizers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MeetupEntityConfig());
        builder.ApplyConfiguration(new OrganizerEntityConfig());
        builder.ApplyConfiguration(new PlaceEntityConfig());
        builder.ApplyConfiguration(new PlanStepEntityConfig());
    }
}