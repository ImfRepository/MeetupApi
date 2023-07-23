namespace Meetup.WebApi.Controllers;

public class ResponseDto<T>
{
	public string Status { get; set; } = null!;

	public List<string>? Errors { get; set; }

	public T? Value { get; set; }
}