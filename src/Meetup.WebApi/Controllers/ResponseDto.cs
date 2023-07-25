namespace Meetup.WebApi.Controllers;

public class ResponseDto
{
	public string Status { get; set; } = null!;

	public List<string>? Errors { get; set; }
}

public class ValuedResponseDto<T>
{
	public string Status { get; set; } = null!;

	public List<string>? Errors { get; set; }

	public T? Value { get; set; }
}