using Meetup.Core.Domain.Entities;

namespace Meetup.Core.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<MeetupEntity> Meetups { get; }
    public DbSet<PlaceEntity> Places { get; }
    public DbSet<PlanStepEntity> PlanSteps { get; }
    public DbSet<OrganizerEntity> Organizers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}