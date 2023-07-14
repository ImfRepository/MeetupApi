using FluentValidation;
using Meetup.Core.Application.Interfaces;
using Meetup.Core.Domain.Exceptions;
using Meetup.Core.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MeetupController : ControllerBase
{
	private readonly IMeetupRepository _repository;
	private readonly IValidator<MeetupModel> _validator;
	private const string InvalidIdMsg = "Id must be more than 0.";

	public MeetupController(IMeetupRepository repository, IValidator<MeetupModel> validator)
	{
		_repository = repository;
		_validator = validator;
	}

	[HttpGet]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(CancellationToken token)
	{
		try
		{
			var result = await _repository.GetAllOrEmptyAsync(token);
			return result.Match<IActionResult>(Ok, exception =>
			{
				if (exception is RepositoryException)
					return BadRequest(exception.Message);

				return StatusCode(500);
			});
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return StatusCode(500);
		}
	}

	[HttpGet("{id:int}")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get([FromRoute] int id, CancellationToken token)
	{
		try
		{
			if (id <= 0) return BadRequest(InvalidIdMsg);

			var result = await _repository.GetByIdOrEmptyAsync(id, token);
			return result.Match<IActionResult>(Ok, exception =>
			{
				if (exception is RepositoryException)
					return BadRequest(exception.Message);

				return StatusCode(500);
			});
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return StatusCode(500);
		}
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Add([FromBody] MeetupModel meetupModel, CancellationToken token)
	{
		try
		{
			var validationResult = await _validator.ValidateAsync(meetupModel, token);
			if (!validationResult.IsValid) 
				return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

			var result = await _repository.CreateAsync(meetupModel, token);
			return result.Match<IActionResult>(b => Ok(b), exception =>
			{
				if (exception is RepositoryException)
					return BadRequest(exception.Message);

				return StatusCode(500);
			});
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return StatusCode(500);
		}
	}

	[HttpPut]
	[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update([FromBody] MeetupModel meetupModel, CancellationToken token)
	{
		try
		{
			var validationResult = await _validator.ValidateAsync(meetupModel, token);
			if (!validationResult.IsValid) 
				return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

			var result = await _repository.UpdateAsync(meetupModel, token);
			return result.Match<IActionResult>(b => Ok(), exception =>
			{
				if (exception is RepositoryException)
					return BadRequest(exception.Message);

				return StatusCode(500);
			});

		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return StatusCode(500);
		}
	}

	[HttpDelete]
	[Authorize(Roles = "admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken token)
	{
		try
		{
			if (id <= 0) return BadRequest(InvalidIdMsg);

			var result = await _repository.DeleteAsync(id, token);
			return result.Match<IActionResult>(b => Ok(), exception =>
			{
				if (exception is RepositoryException)
					return BadRequest(exception.Message);

				return StatusCode(500);
			});
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return StatusCode(500);
		}
	}
}