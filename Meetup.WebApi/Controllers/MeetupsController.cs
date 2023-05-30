using Meetup.Core.Domain;
using Meetup.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MeetupsController : ControllerBase
	{
		private readonly IEventRepository _repository;

		public MeetupsController(IEventRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult> Get([FromQuery] int? id, CancellationToken token)
		{
			try
			{
				return id != null ? Ok(await _repository.GetByIdAsync(id ?? 0, token)) 
					: Ok(await _repository.GetAllAsync(token));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] Event meetup, CancellationToken token)
		{
			return await _repository.CreateAsync(meetup, token) ? Ok() : BadRequest();
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] Event meetup, CancellationToken token)
		{
			return await _repository.UpdateAsync(meetup, token) ? Ok() : BadRequest();
		}

		[HttpDelete]
		public async Task<bool> Delete([FromQuery] int id, CancellationToken token)
		{
			return await _repository.DeleteAsync(id, token);
		}

	}
}