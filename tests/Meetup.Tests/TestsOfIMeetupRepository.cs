using Meetup.Core.Domain.Entities;
using Meetup.Infrastructure;

namespace Meetup.Tests;

/*public class TestsOfIMeetupRepository
{
	private static readonly Random Random = new();

	private static readonly DateTime BaseTime = new DateTime(2035, 8, 1,
			12, 0, 0)
		.ToUniversalTime();

	private readonly List<int> _ids = new();
	private readonly IServiceProvider _services;

	public TestsOfIMeetupRepository()
	{
		var config = new ConfigurationBuilder()
			.AddUserSecrets<TestConfig>()
			.Build();

		_services = new ServiceCollection()
			.AddInfrastructure(config)
			.BuildServiceProvider();

		AsyncPreparations().Wait();
	}

	private static MeetupModel RandomMeetupModel =>
		new()
		{
			Name = RandomString(10),
			Description = RandomString(20),
			Organizer = RandomString(10),
			Speaker = RandomString(10),
			Place = RandomString(5),
			Time = BaseTime,
			Plan = new Dictionary<DateTime, string>
			{
				{ BaseTime, RandomString(8) },
				{ BaseTime + TimeSpan.FromMinutes(15), RandomString(8) },
				{ BaseTime + TimeSpan.FromMinutes(30), RandomString(8) }
			}
		};

	private async Task AsyncPreparations()
	{
		var repo = GetRepository();

		// Clearing db
		var ids = (await repo.GetAllOrEmptyAsync()).Select(e => e.Id).ToList();
		foreach (var id in ids) await repo.DeleteAsync(id);

		// Sealed data adding
		for (var i = 0; i < 5; i++) await repo.CreateAsync(RandomMeetupModel);

		// Saving ids for no concurrency in tests
		_ids.AddRange((await repo.GetAllOrEmptyAsync())
			.Select(e => e.Id));
	}

	private static string RandomString(uint length)
	{
		var builder = new StringBuilder();
		for (var i = 0; i < length; i++) builder.Append((char)Random.Next('a', 'z'));

		return builder.ToString();
	}

	private IMeetupRepository GetRepository()
	{
		var repo = _services.GetService<IMeetupRepository>();
		return repo ?? throw new NullReferenceException();
	}

	[Fact]
	public async Task GetAll()
	{
		var repo = GetRepository();

		var meetups = await repo.GetAllOrEmptyAsync();

		Assert.NotEmpty(meetups);
	}

	[Fact]
	public async Task GetById()
	{
		var repo = GetRepository();

		var meetup = await repo.GetByIdOrEmptyAsync(_ids[1]);

		Assert.NotEqual(meetup, MeetupModel.Empty);
	}

	[Fact]
	public async Task Create()
	{
		var repo = GetRepository();
		var expCount = (await repo.GetAllOrEmptyAsync()).Count() + 1;

		var id = await repo.CreateAsync(RandomMeetupModel);
		var actCount = (await repo.GetAllOrEmptyAsync()).Count();

		Assert.Equal(expCount, actCount);
		Assert.True(id > 0);
	}

	[Fact]
	public async Task Update()
	{
		var repo = GetRepository();
		var expCount = (await repo.GetAllOrEmptyAsync()).Count();

		var updated = RandomMeetupModel;
		updated.Id = _ids[2];
		var result = await repo.UpdateAsync(updated);
		var actCount = (await repo.GetAllOrEmptyAsync()).Count();

		Assert.Equal(expCount, actCount);
		Assert.True(result > 0);
	}

	[Fact]
	public async Task Delete()
	{
		var repo = GetRepository();
		var expCount = (await repo.GetAllOrEmptyAsync()).Count() - 1;

		var result = await repo.DeleteAsync(_ids[3]);
		var actCount = (await repo.GetAllOrEmptyAsync()).Count();

		Assert.Equal(expCount, actCount);
		Assert.True(result > 0);
	}
}

internal class TestConfig
{
	public required string PgContext { get; set; }
}
*/