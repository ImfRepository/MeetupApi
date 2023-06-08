using System.ComponentModel.DataAnnotations.Schema;

namespace Meetup.Infrastructure.Data.Models;

public class MeetupDto
{
    [Column("meetup_id")]
    public int Id { get; set; }

    [Column("meetup_name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("organizer_id")]
    public int OrganizerId { get; set; }
    public virtual OrganizerDto OrganizerDto { get; set; }

    [Column("speaker")]
    public string Speaker { get; set; }

    [Column("meetup_time")]
    public DateTime Time { get; set; }

    [Column("place_id")]
    public int PlaceId { get; set; }
    public PlaceDto PlaceDto { get; set; }

    public virtual IEnumerable<PlanStepDto> PlanSteps { get; set; }
}