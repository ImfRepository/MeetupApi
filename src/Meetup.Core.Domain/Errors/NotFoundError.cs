using FluentResults;

namespace Meetup.Core.Domain.Errors;

public class NotFoundError : Error
{
	public NotFoundError()
		: base("No objects found.") {}

	public NotFoundError(string name, string key, string value)
		: base($"{name} with \'{key}\' = \'{value}\' was not found in db.") { }
}