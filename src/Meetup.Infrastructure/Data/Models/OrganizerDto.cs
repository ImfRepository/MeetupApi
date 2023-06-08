using System.ComponentModel.DataAnnotations.Schema;

namespace Meetup.Infrastructure.Data.Models;

public class OrganizerDto
{
    [Column("organizer_id")]
    public int Id { get; set; }

    [Column("organizer_name")]
    public string Name { get; set; }

    public virtual IEnumerable<MeetupDto> Meetups { get; set; }
}