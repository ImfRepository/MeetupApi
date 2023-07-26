using MediatR;
using Meetup.Core.Application;
using Meetup.Core.Application.Data.Meetups.Queries.GetAllMeetups;
using Meetup.Core.Application.Data.PlanSteps.Commands.CreatePlanStep;
using Meetup.Core.Application.Data.PlanSteps.Commands.DeletePlanStep;
using Meetup.Core.Application.Data.PlanSteps.Commands.UpdatePlanStep;
using Meetup.Core.Application.Data.PlanSteps.Queries.GetAllPlanSteps;
using Meetup.Core.Application.Data.PlanSteps.Queries.GetPlanStepById;

namespace Meetup.Tests;

public class PlanStepRequestsTest
{
	private readonly IMediator _mediator;

	public PlanStepRequestsTest()
	{
		var config = new ConfigurationBuilder()
			.AddUserSecrets<TestConfig>()
			.Build();

		var provider = new ServiceCollection()
			.AddApplicationServices()
			.AddInfrastructure(config)
			.BuildServiceProvider();

		_mediator = provider.GetService<IMediator>()
		            ?? throw new NullReferenceException();
	}

	[Fact]
	public async Task GetAll()
	{
		var id = (await _mediator.Send(new GetAllMeetupsQuery()))
			.ValueOrDefault.FirstOrDefault()!.Id;

		var result = await _mediator.Send(new GetAllPlanStepsQuery(id));

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task GetById()
	{
		var id = await GetAnyStepId();

		var result = await _mediator.Send(new GetPlanStepByIdQuery(id));

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Create()
	{
		var id = (await _mediator.Send(new GetAllMeetupsQuery()))
			.ValueOrDefault.FirstOrDefault()!.Id;

		var command = new CreatePlanStepCommand()
		{
			MeetupId = id,
			Name = "First step",
			Time = DateTime.Now
		};

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Update()
	{
		var id = await GetAnyStepId();

		var command = new UpdatePlanStepCommand()
		{
			Id = id,
			Name = "Second step",
			Time = DateTime.Now
		};

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Delete()
	{
		var id = await GetAnyStepId();

		var result = await _mediator.Send(new DeletePlanStepCommand(id));

		Assert.True(result.IsSuccess);
	}

	private async Task<int> GetAnyStepId()
	{
		return (await _mediator.Send(new GetAllMeetupsQuery()))
			.ValueOrDefault
			.FirstOrDefault(e => e.PlanSteps.Any())!
			.PlanSteps
			.FirstOrDefault()!
			.Id;
	}
}