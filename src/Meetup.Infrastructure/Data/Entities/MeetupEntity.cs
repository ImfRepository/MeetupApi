namespace Meetup.Infrastructure.Data.Entities;

public class MeetupEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int OrganizerId { get; set; }
    public OrganizerEntity? Organizer { get; set; }

    public string Speaker { get; set; }

    public DateTime Time { get; set; }

    public int PlaceId { get; set; }
    public PlaceEntity? Place { get; set; }

    public IEnumerable<PlanStepEntity> PlanSteps { get; set; }
}