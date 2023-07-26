using System.Reflection;
using Meetup.Core.Application.Common.Interfaces;
using Meetup.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

internal class PgContext : DbContext, IApplicationDbContext
{
    public PgContext(DbContextOptions<PgContext> opt) : base(opt) { }

    public DbSet<MeetupEntity> Meetups => Set<MeetupEntity>();

    public DbSet<PlaceEntity> Places => Set<PlaceEntity>();

    public DbSet<PlanStepEntity> PlanSteps => Set<PlanStepEntity>();

    public DbSet<OrganizerEntity> Organizers => Set<OrganizerEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
	    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

	    base.OnModelCreating(builder);
	  }
}