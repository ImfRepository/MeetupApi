namespace Meetup.Infrastructure.Data.Entities;

public class PlanStepEntity
{
    public int Id { get; set; }

    public int MeetupId { get; set; }
    public virtual MeetupEntity Meetup { get; set; }

    public DateTime Time { get; set; }

    public string Name { get; set; }
}