using FluentResults;
using Meetup.Core.Application.Common.Extensions;
using Meetup.Core.Domain.Errors;
using Meetup.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Extensions;

public static class ResultExtensions
{
	public static IActionResult ToActionResult<T>(this Result<T> result)
	{
		if (result.IsSuccess)
		{
			return SuccessValuedResult(result);
		}

		var failedResult = new Result()
			.WithErrors(result.Errors);

		return failedResult.ToActionResult();
	}

	public static IActionResult ToActionResult(this Result result)
	{
		if (result.IsSuccess)
		{
			return SuccessResult();
		}

		if (result.IsInvalid())
		{
			return InvalidResult(result);
		}

		if (result.IsExceptional())
		{
			return InternalErrorResult();
		}

		return ErrorResult(result);
	}

	private static IActionResult SuccessValuedResult<T>(IResult<T> result)
	{
		return new OkObjectResult(result.ValueOrDefault);
	}

	private static IActionResult SuccessResult()
	{
		return new OkResult();
	}

	private static IActionResult InvalidResult(IResultBase result)
	{
		var response = new BadResponse()
		{
			Status = "Invalid",
			Errors = result.Errors
				.Where(e => e is ValidationError)
				.Select(e => e.Message)
				.ToList()
		};

		return new BadRequestObjectResult(response);
	}

	private static IActionResult InternalErrorResult()
	{
		return new StatusCodeResult(500);
	}

	private static IActionResult ErrorResult(IResultBase result)
	{
		var response = new BadResponse()
		{
			Status = "Error",
			Errors = result.Errors
				.Where(e => e is not ExceptionalError)
				.Select(e => e.Message)
				.ToList()
		};

		return new BadRequestObjectResult(response);
	}
}