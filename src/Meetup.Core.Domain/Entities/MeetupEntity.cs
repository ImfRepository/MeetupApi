namespace Meetup.Core.Domain.Entities;

public class MeetupEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int OrganizerId { get; set; }

    public virtual OrganizerEntity Organizer { get; set; } = null!;

    public string Speaker { get; set; } = string.Empty;

    public DateTime Time { get; set; }

    public int PlaceId { get; set; }

    public virtual PlaceEntity Place { get; set; } = null!;

    public virtual IEnumerable<PlanStepEntity> PlanSteps { get; set; } = null!;
}