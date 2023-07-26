using MediatR;
using Meetup.Core.Application.Data.Organizers.Commands.CreateOrganizer;
using Meetup.Core.Application.Data.Organizers.Commands.DeleteOrganizer;
using Meetup.Core.Application.Data.Organizers.Commands.UpdateOrganizer;
using Meetup.Core.Application.Data.Organizers.Queries.GetAllOrganizers;
using Meetup.Core.Application.Data.Organizers.Queries.GetOrganizerById;
using Meetup.Core.Application;

namespace Meetup.Tests;

public class OrganizerRequestsTest
{
	private readonly IMediator _mediator;

	public OrganizerRequestsTest()
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
		var result = await _mediator.Send(new GetAllOrganizersQuery());

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task GetById()
	{
		var all = await _mediator.Send(new GetAllOrganizersQuery());

		var id = all.ValueOrDefault.FirstOrDefault()!.Id;

		var result = await _mediator.Send(new GetOrganizerByIdQuery(id));

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Create()
	{
		var time = DateTime.Now;

		var command = new CreateOrganizerCommand($"IBM Robot Organizer {time}");

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Update()
	{
		var organizer = (await _mediator.Send(new GetAllOrganizersQuery()))
			.ValueOrDefault.FirstOrDefault()!;

		var command = new UpdateOrganizerCommand()
		{
			Id = organizer.Id,
			Name = "New IBM Robot Name"
		};

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Delete()
	{
		var id = (await _mediator.Send(new GetAllOrganizersQuery()))
			.ValueOrDefault.FirstOrDefault()!.Id;

		var result = await _mediator.Send(new DeleteOrganizerCommand(id));

		Assert.True(result.IsSuccess);
	}
}