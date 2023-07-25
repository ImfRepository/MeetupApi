using MediatR;
using Meetup.Core.Application.Data.PlanSteps.Commands.CreatePlanStep;
using Meetup.Core.Application.Data.PlanSteps.Commands.DeletePlanStep;
using Meetup.Core.Application.Data.PlanSteps.Commands.UpdatePlanStep;
using Meetup.Core.Application.Data.PlanSteps.Queries.GetAllPlanSteps;
using Meetup.Core.Application.Data.PlanSteps.Queries.GetPlanStepById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Meetup.WebApi.Extensions;

namespace Meetup.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanStepController : ControllerBase
{
	private readonly IMediator _mediator;

	public PlanStepController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> Get([FromBody]GetAllPlanStepsQuery query, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(query, cancellationToken);

		return response.ToActionResult();
	}

	[HttpGet("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetPlanStepByIdQuery(id), cancellationToken);

		return response.ToActionResult();
	}

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Add([FromBody] CreatePlanStepCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpPut]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Update([FromBody] UpdatePlanStepCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return response.ToActionResult();
	}

	[HttpDelete]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new DeletePlanStepCommand(id), cancellationToken);

		return response.ToActionResult();
	}
}