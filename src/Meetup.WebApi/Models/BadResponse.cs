namespace Meetup.WebApi.Models;

public class BadResponse
{
	public string Status { get; set; } = null!;

	public List<string>? Errors { get; set; }
}