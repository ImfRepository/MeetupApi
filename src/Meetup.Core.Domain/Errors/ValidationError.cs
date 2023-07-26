using FluentResults;

namespace Meetup.Core.Domain.Errors;

public class ValidationError : Error
{
	public ValidationError(string message) : base(message) { }
}