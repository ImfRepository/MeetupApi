using MediatR;
using Meetup.Core.Application;
using Meetup.Core.Application.Data.Meetups.Commands.CreateMeetup;
using Meetup.Core.Application.Data.Meetups.Commands.DeleteMeetup;
using Meetup.Core.Application.Data.Meetups.Commands.UpdateMeetup;
using Meetup.Core.Application.Data.Meetups.Queries.GetAllMeetups;
using Meetup.Core.Application.Data.Meetups.Queries.GetMeetupById;
using Meetup.Core.Application.Data.Organizers.Queries.GetAllOrganizers;
using Meetup.Core.Application.Data.Places.Queries.GetAllPlaces;

namespace Meetup.Tests;

public class MeetupRequestsTest
{
	private readonly IMediator _mediator;

	public MeetupRequestsTest()
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
		var result = await _mediator.Send(new GetAllMeetupsQuery());

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task GetById()
	{
		var all = await _mediator.Send(new GetAllMeetupsQuery());

		var id = all.ValueOrDefault.FirstOrDefault()!.Id;

		var result = await _mediator.Send(new GetMeetupByIdQuery(id));

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Create()
	{
		var time = DateTime.Now.ToUniversalTime();

		var orgId = await GetAnyOrganizerId();

		var placeId = await GetAnyPlaceId();

		var command = new CreateMeetupCommand()
		{
			Name = $"Meetup in {time}",
			Description = $"Just a meetup.",
			Speaker = "me",
			Time = time,
			OrganizerId = orgId,
			PlaceId = placeId
		};

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Update()
	{
		var meetup = (await _mediator.Send(new GetAllMeetupsQuery()))
			.ValueOrDefault.FirstOrDefault()!;

		var command = new UpdateMeetupCommand()
		{
			Id = meetup.Id,
			Name = meetup.Name,
			Description = meetup.Description,
			OrganizerId = await GetAnyOrganizerId(),
			PlaceId = await GetAnyPlaceId(),
			Time = DateTime.Now.ToUniversalTime(),
			Speaker = "not me"
		};

		var result = await _mediator.Send(command);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task Delete()
	{
		var id = (await _mediator.Send(new GetAllMeetupsQuery()))
			.ValueOrDefault.LastOrDefault()!.Id;

		var result = await _mediator.Send(new DeleteMeetupCommand(id));

		Assert.True(result.IsSuccess);
	}

	public async Task<int> GetAnyOrganizerId()
	{
		var organizer = (await _mediator.Send(new GetAllOrganizersQuery()))
			.ValueOrDefault
			.ToArray();

		var max = organizer.Length;

		return organizer[Random.Shared.Next(max)].Id;
	}

	public async Task<int> GetAnyPlaceId()
	{
		var places = (await _mediator.Send(new GetAllPlacesQuery()))
			.ValueOrDefault
			.ToArray();

		var max = places.Length;

		return places[Random.Shared.Next(max)].Id;
	}

	private async Task InitializeDb()
	{
		var meetups = await _mediator.Send(new GetAllMeetupsQuery());

		for (var i = meetups.ValueOrDefault?.Count() ?? 0; i < 4; i++)
		{
			await Create();
		}
	}
}

internal class TestConfig
{
	public required string PgContext { get; set; }
}