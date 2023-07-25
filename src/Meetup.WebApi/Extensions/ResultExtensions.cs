using FluentResults;
using Meetup.Core.Application.Common.Extensions;
using Meetup.Core.Domain.Errors;
using Meetup.WebApi.Controllers;
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
		var response = new ValuedResponseDto<T>()
		{
			Status = "Success",
			Value = result.ValueOrDefault
		};

		return new OkObjectResult(response);
	}

	private static IActionResult SuccessResult()
	{
		var response = new ResponseDto
		{
			Status = "Success"
		};

		return new OkObjectResult(response);
	}

	private static IActionResult InvalidResult(IResultBase result)
	{
		var response = new ResponseDto()
		{
			Status = "Invalid",
			Errors = result.Errors
				.Where(e => e is ValidationError)
				.Select(e => e.Message)
				.ToList()
		};

		return new OkObjectResult(response);
	}

	private static IActionResult InternalErrorResult()
	{
		var response = new ResponseDto()
		{
			Status = "Internal error"
		};

		return new OkObjectResult(response);
	}

	private static IActionResult ErrorResult(IResultBase result)
	{
		var response = new ResponseDto()
		{
			Status = "Error",
			Errors = result.Errors
				.Where(e => e is not ExceptionalError)
				.Select(e => e.Message)
				.ToList()
		};

		return new OkObjectResult(response);
	}
}