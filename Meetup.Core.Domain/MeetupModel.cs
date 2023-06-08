namespace Meetup.Core.Domain;

public class MeetupModel
{
	public int? Id { get; set; }

	public string Name { get; set; }
	public string Description { get; set; }
	public Dictionary<DateTime, string> Plan { get; set; }

	public string Organizer { get; set; }
	public string Speaker { get; set; }

	public DateTime Time { get; set; }
	public string Place { get; set; }

	public static readonly MeetupModel Empty = new()
	{
		Id = null,
		Name = string.Empty,
		Description = string.Empty,
		Plan = new Dictionary<DateTime, string>(),
		Organizer = string.Empty,
		Speaker = string.Empty,
		Time = DateTime.MinValue,
		Place = string.Empty
	};
}