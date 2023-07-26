using System.ComponentModel.DataAnnotations.Schema;

namespace Meetup.Infrastructure.Data.Models;

public class PlaceDto
{
    [Column("place_id")]
    public int Id { get; set; }

    [Column("place_name")]
    public string Name { get; set; }

    public virtual IEnumerable<MeetupDto> Meetups { get; set; }
}