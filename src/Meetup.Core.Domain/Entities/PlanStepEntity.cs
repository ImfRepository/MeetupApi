namespace Meetup.Core.Domain.Entities;

public class PlanStepEntity
{
    public int Id { get; set; }

    public int MeetupId { get; set; }

    public virtual MeetupEntity Meetup { get; set; } = null!;

    public DateTime Time { get; set; }

    public string Name { get; set; } = string.Empty;
}