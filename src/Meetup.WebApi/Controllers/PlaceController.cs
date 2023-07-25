using MediatR;
using Meetup.Core.Application.Data.Places.Commands.CreatePlace;
using Meetup.Core.Application.Data.Places.Commands.DeletePlace;
using Meetup.Core.Application.Data.Places.Commands.UpdatePlace;
using Meetup.Core.Application.Data.Places.Queries.GetAllPlaces;
using Meetup.Core.Application.Data.Places.Queries.GetPlaceById;
using Meetup.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
	private readonly IMediator _mediator;

	public PlaceController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetAllPlacesQuery(), cancellationToken);

		return response.ToActionResult();
	}

	[HttpGet("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetPlaceByIdQuery(id), cancellationToken);

		return response.ToActionResult();
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Add([FromBody] CreatePlaceCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpPut]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Update([FromBody] UpdatePlaceCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpDelete]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new DeletePlaceCommand(id), cancellationToken);

		return response.ToActionResult();
	}
}