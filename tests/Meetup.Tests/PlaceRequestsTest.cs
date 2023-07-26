using MediatR;
using Meetup.Core.Application;
using Meetup.Core.Application.Data.Places.Commands.CreatePlace;
using Meetup.Core.Application.Data.Places.Commands.DeletePlace;
using Meetup.Core.Application.Data.Places.Commands.UpdatePlace;
using Meetup.Core.Application.Data.Places.Queries.GetAllPlaces;
using Meetup.Core.Application.Data.Places.Queries.GetPlaceById;

namespace Meetup.Tests;

public class PlaceRequestsTest
{
	private readonly IMediator _mediator;

	public PlaceRequestsTest()
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
		InitializeDb().Wait();
	}

	[Fact]
	public async Task GetAll()
	{
		var result = await _mediator.Send(new GetAllPlacesQuery());

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task GetById()
	{
		var all = await _mediator.Send(new GetAllPlacesQuery());

		var id = all.ValueOrDefault.FirstOrDefault()!.Id;

		var result = await _mediator.Send(new GetPlaceByIdQuery(id));

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Create()
	{
		var command = new CreatePlaceCommand("House of volleyball.");

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Update()
	{
		var place = (await _mediator.Send(new GetAllPlacesQuery()))
			.ValueOrDefault.FirstOrDefault()!;

		var command = new UpdatePlaceCommand()
		{
			Id = place.Id,
			Name = "New horror house."
		};

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Delete()
	{
		var id = (await _mediator.Send(new GetAllPlacesQuery()))
			.ValueOrDefault.FirstOrDefault()!.Id;

		var result = await _mediator.Send(new DeletePlaceCommand(id));

		Assert.True(result.IsSuccess);
	}

	private async Task InitializeDb()
	{
		var places = await _mediator.Send(new GetAllPlacesQuery());

		for (var i = places.ValueOrDefault?.Count() ?? 0; i < 4; i++)
		{
			await Create();
		}
	}
}