using Meetup.Core.Application;
using Meetup.Core.Domain;
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

		[Authorize]
		[HttpGet]
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

		[Authorize]
		[HttpGet("{id:int}")]
		public async Task<ActionResult> Get([FromRoute] int id, CancellationToken token)
		{
			try
			{
				var meetup = await _repository.GetByIdOrEmptyAsync(id, token);
				return meetup != MeetupModel.Empty
					? Ok(meetup)
					: NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] MeetupModel meetupModel, CancellationToken token)
		{
			try
			{
				var id = await _repository.CreateAsync(meetupModel, token);
				return Ok($"Successfully created. Id = {id}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] MeetupModel meetupModel, CancellationToken token)
		{
			try
			{
				return await _repository.UpdateAsync(meetupModel, token) > 0 
					? Ok("Successfully changed.") 
					: BadRequest("MeetupModel with this name is not exists.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}

		}

		[Authorize(Roles = "admin")]
		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken token)
		{
			try
			{
				return await _repository.DeleteAsync(id, token) > 0
					? Ok("Successfully deleted.")
					: BadRequest("MeetupModel with this id is not exists.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}

		}
	}
}