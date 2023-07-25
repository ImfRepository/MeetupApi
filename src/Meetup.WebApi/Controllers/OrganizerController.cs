using MediatR;
using Meetup.Core.Application.Data.Organizer.Commands.CreateOrganizer;
using Meetup.Core.Application.Data.Organizer.Commands.DeleteOrganizer;
using Meetup.Core.Application.Data.Organizer.Commands.UpdateOrganizer;
using Meetup.Core.Application.Data.Organizer.Queries.GetAllOrganizers;
using Meetup.Core.Application.Data.Organizer.Queries.GetOrganizerById;
using Meetup.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizerController : ControllerBase
{
	private readonly IMediator _mediator;

	public OrganizerController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetAllOrganizersQuery(), cancellationToken);

		return response.ToActionResult();
	}

	[HttpGet("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetOrganizerByIdQuery(id), cancellationToken);

		return response.ToActionResult();
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Add([FromBody] CreateOrganizerCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpPut]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Update([FromBody] UpdateOrganizerCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpDelete]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new DeleteOrganizerCommand(id), cancellationToken);

		return response.ToActionResult();
	}
}