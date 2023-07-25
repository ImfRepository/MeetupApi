using MediatR;
using Meetup.Core.Application.Data.Meetups.Commands.DeleteMeetup;
using Meetup.Core.Application.Data.Meetups.Queries.GetAllMeetups;
using Meetup.Core.Application.Data.Meetups.Queries.GetMeetupById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Meetup.WebApi.Extensions;
using Meetup.Core.Application.Data.Meetups.Commands.CreateMeetup;
using Meetup.Core.Application.Data.Meetups.Commands.UpdateMeetup;

namespace Meetup.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MeetupController : ControllerBase
{
	private readonly IMediator _mediator; 

	public MeetupController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	//[Authorize]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetAllMeetupsQuery(), cancellationToken);

		return response.ToActionResult();
	}

	[HttpGet("{id:int}")]
	//[Authorize]
	public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetMeetupByIdQuery(id), cancellationToken);

		return response.ToActionResult();
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Add([FromBody] CreateMeetupCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpPut]
	[Authorize(Roles = "admin")]

	public async Task<IActionResult> Update([FromBody] UpdateMeetupCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpDelete]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new DeleteMeetupCommand(id), cancellationToken);

		return response.ToActionResult();
	}
}