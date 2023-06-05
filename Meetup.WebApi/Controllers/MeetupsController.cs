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
				var meetups = await _repository.GetAllAsync(token);
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
				var meetup = await _repository.GetByIdAsync(id, token);
				return meetup != MeetupView.Empty
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
		public async Task<IActionResult> Add([FromBody] MeetupView meetupView, CancellationToken token)
		{
			try
			{
				return await _repository.CreateAsync(meetupView, token) 
					? Ok("Successfully created.") 
					: BadRequest("MeetupView with this name already exists.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] MeetupView meetupView, CancellationToken token)
		{
			try
			{
				return await _repository.UpdateAsync(meetupView, token) 
					? Ok("Successfully changed.") 
					: BadRequest("MeetupView with this name is not exists.");
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
				return await _repository.DeleteAsync(id, token) 
					? Ok("Successfully deleted.")
					: BadRequest("MeetupView with this id is not exists.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest("Failed to handle request. Try again later.");
			}

		}
	}
}