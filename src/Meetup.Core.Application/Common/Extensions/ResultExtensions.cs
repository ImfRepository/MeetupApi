namespace Meetup.Core.Application.Common.Extensions;

public static class ResultExtensions
{
	public static bool IsInvalid(this ResultBase result)
	{
		return result.Errors
			.Any(e => e is ValidationError);
	}

	public static bool IsExceptional(this ResultBase result)
	{
		return result.Errors
			.Any(e => e is ExceptionalError);
	}
}