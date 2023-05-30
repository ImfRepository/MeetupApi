namespace Meetup.Core.Domain;

public class Event
{
	public int? Id { get; set; }

	public string Name { get; set; }
	public string Description { get; set; }
	public Dictionary<DateTime, string> Plan { get; set; }

	public string Organizer { get; set; }
	public string Speaker { get; set; }

	public DateTime Time { get; set; }
	public string Place { get; set; }
}