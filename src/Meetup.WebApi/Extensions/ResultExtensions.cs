using FluentResults;
using Meetup.Core.Domain.Errors;
using Meetup.WebApi.Controllers;

namespace Meetup.WebApi.Extensions;

public static class ResultExtensions
{
	public static ResponseDto<T> ToResponseDto<T>(this Result<T> result)
	{
		var response = new ResponseDto<T>();

		if (result.IsSuccess)
		{
			response.Status = "Ok";
			response.Value = result.ValueOrDefault;
			return response;
		}

		if (result.Errors.Any(e => e is ValidationError))
		{
			response.Status = "Invalid";
			response.Errors = result.Errors
				.Where(e => e is ValidationError)
				.Select(e => e.Message)
				.ToList();

			return response;
		}

		response.Status = "Error";
		response.Errors = result.Errors
			.Where(e => e is not ExceptionalError)
			.Select(e => e.Message)
			.ToList();

		return response;
	}

	public static ResponseDto<bool> ToResponseDto(this Result result)
	{
		var response = new ResponseDto<bool>();

		if (result.IsSuccess)
		{
			response.Status = "Ok";
			return response;
		}

		if (result.Errors.Any(e => e is ValidationError))
		{
			response.Status = "Invalid";
			response.Errors = result.Errors
				.Where(e => e is ValidationError)
				.Select(e => e.Message)
				.ToList();

			return response;
		}

		response.Status = "Error";
		response.Errors = result.Errors
			.Where(e => e is not ExceptionalError)
			.Select(e => e.Message)
			.ToList();

		return response;
	}


}