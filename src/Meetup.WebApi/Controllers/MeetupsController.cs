using Meetup.Core.Application.Interfaces;
using Meetup.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MeetupController : ControllerBase
	{
		private readonly IMeetupRepository _repository;

		public MeetupController(IMeetupRepository repository)
		{
			_repository = repository;
		}

		[HttpGet, Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Get(CancellationToken token)
		{
			try
			{
				var meetups = await _repository.GetAllOrEmptyAsync(token);
				return meetups.Any() 
					? Ok(meetups) 
					: NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}
		}

		[HttpGet("{id:int}"), Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Get([FromRoute] int id, CancellationToken token)
		{
			try
			{
				var meetup = await _repository.GetByIdOrEmptyAsync(id, token);
				return meetup != MeetupEntity.Empty
					? Ok(meetup)
					: NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}
		}

		[HttpPost, Authorize(Roles = "admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Add([FromBody] MeetupEntity meetupEntity, CancellationToken token)
		{
			try
			{
				var id = await _repository.CreateAsync(meetupEntity, token);
				return Ok($"Successfully created. Id = {id}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}
		}

		[HttpPut, Authorize(Roles = "admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromBody] MeetupEntity meetupEntity, CancellationToken token)
		{
			try
			{
				return await _repository.UpdateAsync(meetupEntity, token) > 0 
					? Ok("Successfully changed.") 
					: BadRequest("MeetupEntity with this name is not exists.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}

		}

		[HttpDelete, Authorize(Roles = "admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken token)
		{
			try
			{
				return await _repository.DeleteAsync(id, token) > 0
					? Ok("Successfully deleted.")
					: BadRequest("MeetupEntity with this id is not exists.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}

		}
	}
}