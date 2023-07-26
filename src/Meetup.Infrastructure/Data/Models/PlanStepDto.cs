using System.ComponentModel.DataAnnotations.Schema;

namespace Meetup.Infrastructure.Data.Models;

public class PlanStepDto
{
    [Column("step_id")]
    public int Id { get; set; }

    [Column("meetup_id")]
    public int MeetupId { get; set; }
    public virtual MeetupDto Meetup { get; set; }

    [Column("step_time")]
    public DateTime Time { get; set; }

    [Column("step_name")]
    public string Name { get; set; }
}