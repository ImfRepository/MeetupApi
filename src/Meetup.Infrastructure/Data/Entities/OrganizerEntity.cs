namespace Meetup.Infrastructure.Data.Entities;

public class OrganizerEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual IEnumerable<MeetupEntity> Meetups { get; set; }
}