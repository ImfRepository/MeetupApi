using System.ComponentModel.DataAnnotations.Schema;

namespace Meetup.Infrastructure.SQL;

public class EventEntity
{
    [Column("event_id")]
    public int Id { get; set; }

    [Column("event_name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("organizer_id")]
    public int OrganizerId { get; set; }
    public virtual Organizer Organizer { get; set; }

    [Column("speaker")]
    public string Speaker { get; set; }

    [Column("event_time")]
    public DateTime Time { get; set; }

    [Column("place_id")]
    public int PlaceId { get; set; }
    public Place Place { get; set; }

    public virtual IEnumerable<PlanStep> PlanSteps { get; set; }
}

public class Place
{
    [Column("place_id")]
    public int Id { get; set; }

    [Column("place_name")]
    public string Name { get; set; }

    public virtual IEnumerable<EventEntity> Events { get; set; }
}

public class Organizer
{
    [Column("organizer_id")]
    public int Id { get; set; }

    [Column("organizer_name")]
    public string Name { get; set; }

    public virtual IEnumerable<EventEntity> Events { get; set; }
}

public class PlanStep
{
    [Column("step_id")]
    public int Id { get; set; }

    [Column("event_id")]
    public int EventId { get; set; }
    public virtual EventEntity Event { get; set; }

    [Column("step_time")]
    public DateTime Time { get; set; }

    [Column("step_name")]
    public string Name { get; set; }
}